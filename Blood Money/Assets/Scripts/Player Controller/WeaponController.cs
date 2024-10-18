using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerInputController playerInput;

    //The scriptable object holding all the weapon's information
    public GunData data;

    //The point where the bullet will be instantiated
    [SerializeField] Transform firePoint;

    //The point where the gun sprite will be instantiated
    [SerializeField] Transform gunPos;


    [SerializeField] LineRenderer line;

    float currAmmo;

    GameObject gunObj;

    bool isReloading = false;
    bool canFire = true;

    [SerializeField] LayerMask damageableLayers;


    private void Start()
    {
        gunObj = Instantiate(data.gunObj, gunPos);

        firePoint = gunObj.transform.Find("FirePoint").transform;
        currAmmo = data.ammo;
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleSceneManager.instance.isStarting) return;


        //If the weapon can be fired and the firing input is being pressed
        if (playerInput.didFire && canFire && currAmmo >= 1)
        {
            StartCoroutine(FireWeapon());
        }

        if (currAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator FireWeapon()
    {
        canFire = false;
        currAmmo--;

        if (data.name == "Laser Gun")
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);

            RaycastHit2D raycastHit = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, 1 << 9);
            if (raycastHit)
            {
                Debug.Log("hit");
                line.SetPosition(1, raycastHit.point);
            }

            else
            {
                line.SetPosition(1, transform.position);
            }
        }

        yield return new WaitForSeconds(data.chargeUpTime);

        Instantiate(data.muzzleFlash, firePoint);

        line.enabled = false;

        AudioManager manager = FindFirstObjectByType<AudioManager>();
        manager.Play(data.sfxName);

        if (data.isHitscan)
        {
            //Do Hitscan Shot
            for (int i = 0; i < data.burstCount; i++)
            {
                HitscanShot();
                yield return new WaitForSeconds(data.timeBetweenShots);
            }
        }
        else
        {
            //Projectile Shot
            for (int i = 0; i < data.burstCount; i++)
            {
                ProjectileShot();
                yield return new WaitForSeconds(data.timeBetweenShots);
            }
        }
        yield return new WaitForSeconds(data.fireSpeed);
        canFire = true;
    }

    private void HitscanShot()
    {
        for (int i = 0; i < data.bulletCount; i++)
        {

            if (data.hitscanSize > 0)
            {
                RaycastHit2D[] hitObj = Physics2D.CircleCastAll(firePoint.position, data.hitscanSize, transform.forward, Mathf.Infinity);
                if (hitObj.Length > 0)
                {
                    foreach (RaycastHit2D hit in hitObj)
                    {

                        PlayerHealthManager enemyHealthManager = hit.collider.gameObject.GetComponentInParent<PlayerHealthManager>();

                        if (enemyHealthManager != null && enemyHealthManager.gameObject != gameObject)
                        {
                            Debug.Log("did damage");

                            if (data.hasDamageFallOff)
                            {
                                float dmgFallOff;
                                float distanceFromPlayer = (hit.collider.gameObject.transform.position - transform.position).normalized.magnitude;
                                dmgFallOff = data.damageFalloffMultiplier / distanceFromPlayer;

                                if (dmgFallOff > 1) dmgFallOff = 1;

                                enemyHealthManager.TakeDamage(data.damage * dmgFallOff);
                            }
                            else
                            {
                                enemyHealthManager.TakeDamage(data.damage);
                            }


                        }
                    }
                }
            }
            else
            {
                Debug.Log(data.name);
                RaycastHit2D[] hitObj = Physics2D.RaycastAll(firePoint.position, transform.right, Mathf.Infinity);
                if (hitObj.Length > 0)
                {


                    foreach (RaycastHit2D hit in hitObj)
                    {
                        PlayerHealthManager enemyHealthManager = hit.collider.gameObject.GetComponentInParent<PlayerHealthManager>();

                        if (enemyHealthManager != null && enemyHealthManager.gameObject != gameObject)
                        {
                            if (data.hasDamageFallOff)
                            {
                                float dmgFallOff;
                                float distanceFromPlayer = (hit.collider.gameObject.transform.position - transform.position).normalized.magnitude;
                                dmgFallOff = data.damageFalloffMultiplier / distanceFromPlayer;

                                if (dmgFallOff > 1) dmgFallOff = 1;

                                enemyHealthManager.TakeDamage(data.damage * dmgFallOff);
                            }
                            else
                            {
                                enemyHealthManager.TakeDamage(data.damage);
                            }

                        }
                    }
                }
            }
        }
    }

    private void ProjectileShot()
    {
        for (int i = 0; i < data.bulletCount; i++)
        {
            //Instantiate the bullet and get a reference of the bullet
            GameObject bullet = Instantiate(data.projectileObj, firePoint.transform);

            bullet.transform.parent = null;

            //Get a reference to the script on the bullet that was initiated
            Projectile bulletProjectile = bullet.GetComponent<Projectile>();


            //Initialize the bullet (the hard way)
            bulletProjectile.rb.AddForce(new Vector2(transform.localScale.x, 0) * data.bulletMoveSpeed,
                ForceMode2D.Impulse);

            bulletProjectile.owner = this.gameObject;

            bulletProjectile.bulletDamage = data.damage;

        }
    }

    /// <summary>
    /// Change what gun is currently being used
    /// </summary>
    /// <param name="_newGun">the gun profile that is being switched to</param>
    public void ChangeGun(GunData _newGun)
    {
        data = _newGun;

        Destroy(gunObj.gameObject);

        gunObj = Instantiate(data.gunObj, gunPos);
        Debug.Log(gunObj.name);
        firePoint = gunObj.transform.Find("FirePoint").transform;

        currAmmo = data.ammo;
    }

    /// <summary>
    /// Reload the currently active weapon
    /// </summary>
    /// <returns>waits for the defined time to reload</returns>
    private IEnumerator Reload()
    {
        canFire = false;

        yield return new WaitForSeconds(data.reloadTime);

        currAmmo = data.ammo;
        isReloading = false;
        canFire = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
