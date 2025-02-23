using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] int enemiesInWave;
    [SerializeField] int spawnedEnemies;
    [SerializeField] float spawnDelay = 2f;
    [SerializeField] GameObject enemy;

    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    void Update()
    {
        spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
    }

    IEnumerator EnemySpawn()
    {
        while (spawnedEnemies < enemiesInWave)
        {
            Instantiate(enemy, transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
