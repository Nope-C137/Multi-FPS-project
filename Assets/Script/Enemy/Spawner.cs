using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<GameObject> enemyPrefabs;         // Assign multiple enemy prefabs in Inspector

    [Header("Wave Settings")]
    public int maxTotalEnemies = 1000;            // Maximum enemies to spawn across all waves
    public int initialWaveSize = 100;             // Starting number of enemies
    public float spawnRadius = 300f;               // Radius around the player
    public float delayBeforeNextWave = 3f;        // Delay before next wave starts

    [Header("Player Reference")]
    public Transform player;                      // Assign player Transform in Inspector

    private int currentWaveSize;
    private int totalSpawnedEnemies = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isSpawning = false;

    private void Start()
    {
        currentWaveSize = initialWaveSize;
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning && AllEnemiesCleared())
        {
            if (totalSpawnedEnemies >= maxTotalEnemies)
            {
                Debug.Log("? All waves completed!");
                return;
            }

            // Increase wave size, but don't exceed maxTotalEnemies
            currentWaveSize = Mathf.Min(currentWaveSize + 100, maxTotalEnemies - totalSpawnedEnemies);
            StartCoroutine(StartWave());
        }
    }

    IEnumerator StartWave()
    {
        isSpawning = true;
        Debug.Log($"?? Starting Wave: {currentWaveSize} enemies");

        for (int i = 0; i < currentWaveSize && totalSpawnedEnemies < maxTotalEnemies; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0;
            Vector3 spawnPos = player.position + offset;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            totalSpawnedEnemies++;
        }

        yield return new WaitForSeconds(delayBeforeNextWave);
        isSpawning = false;
    }

    bool AllEnemiesCleared()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
                return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }

}
