using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health { get; set; }
    public int impactDamage { get; set; }
    public float speed { get; set; }

    public void TakeDamage(int damage) {
        health -= damage;

        // Check if the enemy is dead
        if (health <= 0)
        {
            // Destroy the enemy
            Destroy(gameObject);
        }
    }
}
