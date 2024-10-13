using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : Projectile
{
    public float explosionRadius = 10f;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject explosionParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayers);

        for (int i = 0; i < col.Length; i++)
        {
            PlayerHealthManager enemyHealthManager = col[i].gameObject.GetComponentInParent<PlayerHealthManager>();

            Debug.Log(col[i].gameObject.name);

            if (enemyHealthManager != null && col[i] != owner)
            {
                enemyHealthManager.TakeDamage(bulletDamage);
            }
        }

        if (collision.gameObject != owner)
        {
            Destroy(this.gameObject);
        }
    }
}
