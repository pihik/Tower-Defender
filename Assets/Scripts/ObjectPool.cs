using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	[Tooltip ("This amount is set for easy difficulty, it can be more on higher difficulty")]
	[SerializeField] int poolSize = 20;
	[SerializeField] int timeBetweenSpawns = 2;

	Queue<GameObject> pool = new Queue<GameObject>();

	void Awake()
	{
		int diffuculty = GameManager.instance.GetDifficulty() + 1;
		poolSize = Mathf.Max(1, poolSize * diffuculty);
	}

	void Start()
	{
		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
			obj.SetActive(false);
			pool.Enqueue(obj);
		}
	}

	public GameObject GetObjectFromPool()
	{
		if (pool.Count > 0)
		{
			GameObject obj = pool.Dequeue();
			obj.SetActive(true);

			return obj;
		}
		else
		{
			GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
			obj.SetActive(true);

			return obj;
		}
	}

	public void ReturnObjectToPool(GameObject obj)
	{
		obj.SetActive(false);
		pool.Enqueue(obj);
	}

	Transform GetPoolStorage()
	{
		return InGameHelper.instance.GetDefaultStorage();
	}

	public int GetTimeBetweenSpawns()
	{
		return timeBetweenSpawns;
	}

	public int GetPoolSize()
	{
		return poolSize;
	}
}
