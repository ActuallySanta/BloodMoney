using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bulletLifetime = 0f;

    public GameObject owner;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Public initializer for the projectile Game Object
    /// </summary>
    /// <param name="_shotSpeed">How fast the bullet is moving when created</param>
    /// <param name="_direction">What direction the bullet should move in</param>
    /// <param name="_bulletLifeTime"> How long the bullet will exist for</param>
    /// <param name="_owner">The game object that created it</param>
    public void InitializeBullet(float _shotSpeed, Vector2 _direction,float _bulletLifeTime, GameObject _owner)
    {
        Debug.Log("Initialized");

        //Add force to the game object in the direction the player is facing
        rb.AddForce(_direction * _shotSpeed, ForceMode2D.Impulse);

        //Set how long the bullet will exist for
        bulletLifetime = _bulletLifeTime;

        //Set the owner of the bullet
        owner = _owner;

        //Start the timer for the lifetime of the bullet
        StartCoroutine(DestroyBullet());
    }

    /// <summary>
    /// Destroy the bullet after a set amount of time
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bulletLifetime);
        Destroy(this.gameObject);
    }
}
