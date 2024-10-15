using UnityEngine;

public class KillArea : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager healthManager = collision.GetComponentInParent<PlayerHealthManager>();

        if (healthManager != null)
        {
            healthManager.TakeDamage(100000f);
        }
    }
}
