using UnityEngine;
using UnityEngine.U2D.IK;

public class BoucePad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);

            rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
