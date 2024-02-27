using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Check if the enemy is dead
        if (health <= 0)
        {
            // Destroy the enemy
            Destroy(gameObject);
        }
    }
}
