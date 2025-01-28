using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") ||
            other.gameObject.CompareTag("Meteor1") ||
            other.gameObject.CompareTag("Meteor2"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}