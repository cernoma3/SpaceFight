using UnityEngine;

public class Meteor2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float defaultSpeed = 1.5f;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDirection * defaultSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        if (rb != null)
        {
            Vector2 currentDirection = rb.linearVelocity.normalized;
            rb.linearVelocity = currentDirection * newSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Explosion();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("BulletEnemy"))
        {
            Explosion();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void Explosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}