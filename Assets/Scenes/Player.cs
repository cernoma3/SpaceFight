using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;

    [Header("Player Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float respawnTimeout = 5f;
    [SerializeField] private float safeRadius = 1f;

    private Rigidbody2D rb;
    private float fireTimer;

    private Vector2 screenBounds;
    private int currentLives;

    private UIManager uiManager;
    private GameOverManager gameOverManager;

    private void Start()
    {
        Debug.Log("Initializing Player...");
        rb = GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        currentLives = maxLives;

        uiManager = FindObjectOfType<UIManager>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (uiManager != null)
        {
            uiManager.UpdateLives(currentLives);
        }
        else
        {
            Debug.LogError("UIManager not found in the scene!");
        }
    }

    private void Update()
    {
        HandleInput();
        HandleShooting();
        WrapPosition();
    }

    private void HandleInput()
    {
        float rotationInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustForce);
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void WrapPosition()
    {
        Vector3 position = transform.position;

        if (position.x > screenBounds.x)
        {
            position.x = -screenBounds.x;
            ResetRotation();
        }
            
        else if (position.x < -screenBounds.x)
        {
            position.x = screenBounds.x;
            ResetRotation();
        }

        if (position.y > screenBounds.y)
        {
            position.y = -screenBounds.y;
            ResetRotation();
        }
            
        else if (position.y < -screenBounds.y)
        {
            position.y = screenBounds.y;
            ResetRotation();
        }

        transform.position = position;
    }

    private void ResetRotation()
    {
        rb.angularVelocity = 0f;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Invincible"))
        {
            Debug.Log("Player is invincible, ignoring collision.");
            return;
        }

        Debug.Log($"Collision detected with: {other.gameObject.tag}");
        if (other.gameObject.CompareTag("BulletEnemy") ||
            other.gameObject.CompareTag("Enemy") ||
            other.gameObject.CompareTag("Meteor1") ||
            other.gameObject.CompareTag("Meteor2"))
        {
            Debug.Log("Player collided with dangerous object, exploding...");
            Explode();
            HandleDeath();
        }
    }

    private void Explode()
    {
        Debug.Log("Player exploding...");
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void HandleDeath()
    {
        currentLives--;
        Debug.Log($"Player lost a life. Remaining lives: {currentLives}");

        if (uiManager != null)
        {
            uiManager.UpdateLives(currentLives);
        }
        else
        {
            Debug.LogError("UIManager is not assigned or found.");
        }

        if (currentLives <= 0)
        {
            Debug.Log("Game Over! No lives remaining.");
            gameObject.SetActive(false);

            int finalScore = uiManager != null ? uiManager.GetScore() : 0;
            Debug.Log($"Final score: {finalScore}");
            gameOverManager?.ShowGameOver(finalScore);
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawning player...");
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        rb.angularVelocity = 0f;
        gameObject.SetActive(true);
        StartCoroutine(TemporaryInvincibility());
    }

    private IEnumerator TemporaryInvincibility()
    {
        Debug.Log("Player is temporarily invincible.");
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(3f);

        if (collider != null)
        {
            collider.enabled = true;
        }

        Debug.Log("Player is no longer invincible.");
    }
}