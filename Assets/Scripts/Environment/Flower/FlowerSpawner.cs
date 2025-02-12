using System.Collections;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [Header("Flower Spawning Settings")]
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private int totalFlowers = 20;
    [SerializeField] private float spawnDelay = 1f; 
    [SerializeField] private float spawnRadius = 5f; 
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkRadius = 0.5f; 

    private Vector2 playerPosition;

    public void StartSpawning(Vector2 startPosition)
    {
        playerPosition = startPosition;
        StartCoroutine(SpawnFlowers());
    }

    private IEnumerator SpawnFlowers()
    {
        int attempts = 0;

        for (int i = 0; i < totalFlowers; i++)
        {
            Vector2 spawnPosition;
            bool positionIsValid;

            do
            {
                spawnPosition = playerPosition + Random.insideUnitCircle * spawnRadius;
                positionIsValid = !Physics2D.OverlapCircle(spawnPosition, checkRadius, obstacleLayer);
                attempts++;

                // Prevent infinite loops
                if (attempts > 50)  
                {
                    Debug.LogWarning("Couldn't find a valid spawn position for a flower.");
                    yield break;
                }

            } while (!positionIsValid);

            attempts = 0;

            Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}