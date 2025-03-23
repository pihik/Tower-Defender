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

	void OnEnable()
	{
		GameManager.instance.OnDifficultyChanged += OnDifficultyChanged;
	}

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

		OnDifficultyChanged(GameManager.instance.GetDifficulty());
		SetAmountOfEnemies();

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

	public void OnDifficultyChanged(int difficulty)
	{
		switch (difficulty)
		{
			case 0:
				spawnDelay = 8f;
				DevideAmounts(3);
				break;
			case 1:
				spawnDelay = 5f;
				DevideAmounts(2);
				break;
			case 2:
				spawnDelay = 3f;
				break;
			default:
				Debug.LogError("[Spawner::OnDifficultyChanged] Invalid difficulty");
				break;
		}
	}

	void DevideAmounts(int devider)
	{
		if (devider <= 0)
		{
			return;
		}

		for (int i = 0; i < amounts.Count; i++)
		{
			amounts[i] = Mathf.Max(1, amounts[i] / devider);
		}
	}

	void SetAmountOfEnemies()
	{
		int allUnits = 0;

		foreach (var amount in amounts)
		{
			allUnits += amount;
		}

		GameManager.instance.SetNumberOfEnemies(allUnits);
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

	void OnDisable()
	{
		GameManager.instance.OnDifficultyChanged -= OnDifficultyChanged;
	}
}
