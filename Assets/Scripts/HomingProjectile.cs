using UnityEngine;

public class HomingProjectile : MonoBehaviour, IProjectile
{
    public float speed = 10f;
    public int damage = 1;

    private Transform target;
    private Vector3 direction;

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            direction = (target.position - transform.position).normalized;

            // Check if the bullet has reached the target
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
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
        else
        {
            // Continue in the same direction if target is null
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnBecameInvisible()
    {
        // Destroy the bullet when it goes out of view
        Destroy(gameObject);
    }
}
