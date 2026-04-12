using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<GameObject> enemyPrefabs;
    public int enemyCount;
    public float spawnRadius;
    public float delayBeforeNextWave;

}
