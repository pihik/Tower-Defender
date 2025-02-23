using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyHealth))]

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 50;
    [SerializeField] int goldPenalty = 50;

    ShopManager shopManager;

    void Start()
    {
        shopManager = ShopManager.instance;
    }

    public void RewardCoins()
    {
        shopManager.Deposit(goldReward);
    }

    public void StealGold()
    {
        shopManager.WithDraw(goldPenalty);
    }
}
