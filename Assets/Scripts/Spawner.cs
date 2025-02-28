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

    [SerializeField] float spawnDelay = 2f;
    [SerializeField] bool spawnRandomEnemy = true;
    [SerializeField] GameObject[] enemies;
    [Tooltip("This list represents spawning amount for object in array on same position")]
    [SerializeField] List<int> amounts = new List<int>();

    int currentIndex = 0;

    Coroutine spawnCoroutine;

    void Start()
    {
        if (enemies.Length == 0)
        {
            Debug.LogError("[Spawner::Start] Enemies array is empty");
            return;
        }

        if (amounts.Count == 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                int randomNumber = UnityEngine.Random.Range(1, 10);
                amounts.Add(randomNumber);
            }
        }

        spawnCoroutine = StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (IsIndexValid())
        {
            Spawn();
            yield return new WaitForSeconds(spawnDelay);         
        }
    }

    void Spawn()
    {
        Instantiate(enemies[currentIndex], transform);
        amounts[currentIndex]--;
    }

    bool IsIndexValid()
    {
        if (amounts[currentIndex] <= 0)
        {
            currentIndex++;
        }

        if (enemies.Length <= currentIndex && spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            return false;
        }

        return true;
    }
}
