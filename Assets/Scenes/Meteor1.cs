using UnityEngine;

public class Meteor1 : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private GameObject smallerMeteorPrefab;
    [SerializeField] private int meteorSize = 3;
    [SerializeField] private float speed = 1f;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;

    public System.Action OnDestroyAction;

    private Rigidbody2D rb;

    public void SetDirection(Vector3 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * rb.linearVelocity.magnitude;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") ||
            collision.gameObject.CompareTag("BulletEnemy") ||
            collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Player"))
        {
            SpawnSmallerMeteors();

            Explode();

            if (!collision.gameObject.CompareTag("Player"))
            {
                Destroy(collision.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void SpawnSmallerMeteors()
    {
        if (meteorSize > 1 && smallerMeteorPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject smallerMeteor = Instantiate(smallerMeteorPrefab, transform.position, Quaternion.identity);
                Meteor1 smallerMeteorScript = smallerMeteor.GetComponent<Meteor1>();
                if (smallerMeteorScript != null)
                {
                    smallerMeteorScript.meteorSize = meteorSize - 1;
                    smallerMeteorScript.speed = speed * 1.5f;
                }
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        OnDestroyAction?.Invoke();
    }
}