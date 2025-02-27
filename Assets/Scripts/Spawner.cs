using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Singleton
    public static Spawner instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] int enemiesInWave;
    [SerializeField] int spawnedEnemies;
    [SerializeField] float spawnDelay = 2f;
    [SerializeField] GameObject enemy;

    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (spawnedEnemies < enemiesInWave)
        {
            Spawn();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void Spawn()
    {
        Instantiate(enemy, transform);
        spawnedEnemies++;
    }
}
