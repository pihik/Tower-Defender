using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tower : MonoBehaviour
{
    [SerializeField] DefenderStats stats;

    ProjectileScript projectileScript;

    bool isBuilding = true;

    void Awake()
    {
        projectileScript = GetComponentInChildren<ProjectileScript>();

        if (!stats || !projectileScript)
        {
            Debug.LogError("[Tower::Awake] Something went wrong on: " + name);
            return;
        }
    }

    void Start()
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        ShopManager shopManager = ShopManager.instance;

        if (!shopManager || !stats)
        {
            Debug.LogError("[Tower::CreateTower] ShopManager is missing");
            return false;
        }

        if (shopManager.ActualCoins >= stats.cost)
        {
            shopManager.WithDraw(stats.cost);
            Instantiate(tower.gameObject, position, Quaternion.identity);

            return true;
        }

        return false;
    }

    IEnumerator Build()
    {
        isBuilding = true;
        List<Transform> buildOrder = new List<Transform>();

        foreach (Transform child in transform)
        {
            buildOrder.Add(child);
            child.gameObject.SetActive(false);

            foreach (Transform grandchild in child)
            {
                buildOrder.Add(grandchild);
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach (Transform part in buildOrder)
        {
            part.gameObject.SetActive(true);
            yield return new WaitForSeconds(stats.buildTime / buildOrder.Count);
        }

        isBuilding = false;
    }

    public DefenderStats GetStats()
    {
        return stats;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }
}
