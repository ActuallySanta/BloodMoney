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
    public float bulletDamage;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealthManager enemyHealthManager = collision.gameObject.GetComponentInParent<PlayerHealthManager>();

        if (collision.gameObject != owner)
        {
            Debug.Log(collision.gameObject.name);

            if (enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(bulletDamage);
            }

            Destroy(this.gameObject);
        }
    }
}
