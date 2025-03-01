using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] int poolSize = 20;

    Queue<GameObject> pool = new Queue<GameObject>();

    protected virtual void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
            AdditionalInstantiation(obj);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            AdditionalActivation();
            obj.SetActive(true);

            return obj;
        }
        else
        {
            GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
            AdditionalInstantiation(obj);
            AdditionalActivation();
            obj.SetActive(true);

            return obj;
        }
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    protected virtual Transform GetPoolStorage()
    {
        return InGameHelper.instance.GetDefaultStorage();
    }

    protected virtual void AdditionalInstantiation(GameObject obj) { }

    protected virtual void AdditionalActivation() { }
}
