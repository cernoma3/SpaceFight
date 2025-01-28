using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;
    private bool hasScored = false;

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
        Debug.Log($"Bullet collided with: {other.gameObject.tag}");

        if (!hasScored && (other.gameObject.CompareTag("Enemy") ||
                           other.gameObject.CompareTag("Meteor1") ||
                           other.gameObject.CompareTag("Meteor2")))
        {
            Debug.Log("Adding score for destroying an object...");
            var uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene!");
            }
            else
            {
                Debug.Log("Updating score in UIManager.");
                uiManager.UpdateScore(uiManager.GetScore() + 10);
                hasScored = true;
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}