using UnityEngine;

public class HoProjectile : MonoBehaviour, IProjectile
{
    public float speed = 10f;
    public int damage = 1;

    private Transform target;
    private Vector3 moveDirection; // Direction for continuous movement

    void Start()
    {
        // If the projectile has a target at the start, set the move direction towards the target
        if (target != null)
        {
            SetMoveDirection(target.position);
        }
    }

    void Update()
    {
        // Always move in the set direction
        transform.position += moveDirection * speed * Time.deltaTime;

        // Check if the bullet has reached near the original target position
        if (target != null && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Deal damage to the target
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void SetMoveDirection(Vector3 targetPosition)
    {
        // Calculate the direction vector from the current position to the target position
        moveDirection = (targetPosition - transform.position).normalized;
    }

    void OnBecameInvisible()
    {
        // Destroy the bullet when it goes out of view
        Destroy(gameObject);
    }
}
