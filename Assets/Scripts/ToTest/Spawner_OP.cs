using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_OP : MonoBehaviour
{
	[SerializeField] int startDelay = 3;
	[SerializeField] int timeBetweenWaves = 5;

	List<ObjectPool> objectPools = new List<ObjectPool>();
	int currentIndex = 0;
	int timeBetweenSpawns = 2;
	int amountOfEnemiesInPool;

	void Start()
	{
		GetObjectPools();

		if (objectPools.Count == 0)
		{
			Debug.LogError("[Spawner_OP::Start] No Object Pools found under Spawner_OP.");
			return;
		}

		amountOfEnemiesInPool = objectPools[0].GetPoolSize();
		timeBetweenSpawns = objectPools[0].GetTimeBetweenSpawns();

		StartCoroutine(SpawnWaves());
	}

	void GetObjectPools()
	{
		int allUnits = 0;

		foreach (Transform child in transform)
		{
			if (child.TryGetComponent(out ObjectPool pool))
			{
				objectPools.Add(pool);
				allUnits += pool.GetPoolSize();
			}
		}
		GameManager.instance.SetNumberOfEnemies(allUnits);
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startDelay);

		while (true)
		{
			if (amountOfEnemiesInPool <= 0)
			{
				++currentIndex;
				if (currentIndex >= objectPools.Count)
				{
					// spawning finished
					yield break;
				}

				amountOfEnemiesInPool = objectPools[currentIndex].GetPoolSize();
				timeBetweenSpawns = objectPools[currentIndex].GetTimeBetweenSpawns() - 1;
				objectPools[currentIndex].GetObjectFromPool();

				yield return new WaitForSeconds(timeBetweenWaves);
			}

			objectPools[currentIndex].GetObjectFromPool();
			--amountOfEnemiesInPool;

			yield return new WaitForSeconds(timeBetweenSpawns);
		}
	}
}
