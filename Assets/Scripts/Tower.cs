using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int towerCost = 50;
    [SerializeField] float buildTime = 3f;


    void Start()
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Coins coins = FindObjectOfType<Coins>();

        if (coins.ActualCoins >= towerCost)
        {
            Instantiate(tower.gameObject, position, Quaternion.identity);
            coins.WithDraw(towerCost);
            return true;
        }
        return false;
    }
    IEnumerator Build()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTime);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}
