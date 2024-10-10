using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerInputController playerInput;

    //The scriptable object holding all the weapon's information
    [SerializeField] private GunData data;

    //The point where the bullet will be instantiated
    [SerializeField] Transform firePoint;

    //The point where the gun sprite will be instantiated
    [SerializeField] Transform gunPos;

    float currAmmo;

    GameObject gunObj;

    bool isReloading = false;
    bool canFire = true;

    private void Start()
    {
        gunObj = Instantiate(data.gunObj, gunPos);

        firePoint = gunObj.transform.Find("FirePoint").transform;
        currAmmo = data.ammo;
    }

    // Update is called once per frame
    void Update()
    {

        //If the weapon can be fired and the firing input is being pressed
        if (Input.GetKey(playerInput.fireKey) && canFire)
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

        if (data.isHitscan)
        {
            //Do Hitscan Shot
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

            bulletProjectile.bulletLifetime = data.bulletLifetime;
            StartCoroutine(bulletProjectile.DestroyBullet());
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
    }

}
