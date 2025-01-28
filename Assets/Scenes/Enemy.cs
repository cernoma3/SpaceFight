using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    private Rigidbody2D rb;
    public float rotateSpeed = 0.025f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float fireRate = 2f;
    private float fireTimer;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject Explosion;

    public System.Action OnDestroyAction;

    private bool hasInitialized = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = fireRate;

        hasInitialized = true;
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void FixedUpdate()
    {
         MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        RotateTowardsTarget();
        rb.linearVelocity = transform.up * speed;
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector2 targetDirection = (Vector2)(target.position - transform.position);
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void GetTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
        {
            target = player.transform;
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);

        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        Collider2D ownerCollider = GetComponent<Collider2D>();

        if (bulletCollider != null && ownerCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("BulletEnemy"))
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Meteor1"))
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Meteor2"))
        {
            Explode();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
    }
    private void OnDestroy()
    {
        OnDestroyAction?.Invoke();
    }
}
