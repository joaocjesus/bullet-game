using UnityEngine;

public class HomingMine : Enemy
{
    private Vector2 targetPosition;
    private GameObject player;

    void Awake()
    {
        speed = 1f;
        health = 5;
        impactDamage = 5;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        targetPosition = player.transform.position;
    }

    void Update()
    {
        targetPosition = player.transform.position;
        // Move towards the player
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );


        // When target is hit
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Destroys current enemy
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().TakeDamage(impactDamage);
            // Destroys current enemy
            Destroy(gameObject);
        }
    }
}
