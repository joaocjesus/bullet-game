using UnityEngine;

public class GameController : MonoBehaviour
{
    private float enemySpawnTimer;

    public GameObject enemy;
    public float enemySpawnInterval;
    float yBounds;
    float xBounds;

    private void Awake()
    {
        // Calculate bounds with tolerance
        yBounds = Camera.main.orthographicSize + 1;
        xBounds = yBounds * Camera.main.aspect + 1;
    }

    void Start()
    {
        enemySpawnInterval = 5f;
        Instantiate(enemy, GetRandomOuterSpawnVector(), Quaternion.identity);
    }

    void Update()
    {
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            Instantiate(enemy, GetRandomOuterSpawnVector(), Quaternion.identity);
            enemySpawnTimer = 0;
        }
    }

    private Vector2 GetRandomOuterSpawnVector() {
        // To randomise positive or negative edge (-1, 1)
        int randomSign = Random.Range(0, 2) * 2 - 1;
        Debug.Log("randomSign: " + randomSign);

        // To randomise X or Y
        float randomEdge = Random.Range(0, 2);
        Debug.Log("randomEdge: " + randomEdge);

        return randomEdge == 0
            ? new Vector2(getRandomPosition(xBounds), yBounds * randomSign)
            : new Vector2(xBounds * randomSign, getRandomPosition(yBounds));
    }

    private float getRandomPosition(float max) {
        return Random.Range(-max, max);
    }

}
