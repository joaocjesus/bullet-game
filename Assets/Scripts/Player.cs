using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private PlayerInput controls;
    private bool pause = false;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public float shootingInterval;
    private float shootingTimer;

    private List<Transform> enemiesInRange = new List<Transform>();
    public float detectionRange;
    public int health; // Player health

    public int collisionDamage;


    void Awake()
    {
        // Init values
        health = 10;
        collisionDamage = 10;
        speed = 3f;
        shootingInterval = 1f;
        detectionRange = 2f;

        UpdateDetectionRangeVisual();

        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerInput(); // Use GetComponent to get the PlayerInput attached to this GameObject

    }

    void Update()
    {
        if (pause) return;

        shootingTimer += Time.deltaTime;
        //Debug.Log("enemiesInRange.Count: " + enemiesInRange.Count);
        if (shootingTimer >= shootingInterval && enemiesInRange.Count > 0)
        {
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy)
            {
                Shoot(nearestEnemy, bulletPrefab);
                shootingTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collider.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collider.transform);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collisionDamage); // Collision damage
        }
    }

    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle the player's death here
        Debug.Log("Player has died!");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.grey;
        DisableControls();
        pause = true;
    }


    public void Move(InputAction.CallbackContext ctx)
    {
        if (!pause)
        {
            //Debug.Log("Movement: " + ctx);
            rb.velocity = ctx.ReadValue<Vector2>() * speed;
        }
    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    // Method to disable player controls
    public void DisableControls()
    {
        Debug.Log("Disabling player controls");
        controls.Player.Disable();
        rb.velocity = Vector2.zero; // Stop the player's movement
    }

    public void EnableControls()
    {
        // Enable the player controls
        controls.Player.Enable();
    }

    void Shoot(Transform nearestEnemy, GameObject prefab)
    {
        GameObject bullet = Instantiate(prefab, bulletSpawnPoint.position, Quaternion.identity);
        IProjectile projectile = bullet.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.SetTarget(nearestEnemy);
        }
    }

    Transform FindNearestEnemy()
    {
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(enemy.position, currentPosition);
            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }

    private void UpdateDetectionRangeVisual()
    {
        Transform detectionRangeTransform = transform.Find("DetectionRange");
        //Debug.Log("Current detection range: " + detectionRange);

        float scaleFactor = detectionRange * 2;
        //Debug.Log("Setting scale to: " + scaleFactor);

        detectionRangeTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}
