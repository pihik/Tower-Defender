using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyHealth))]

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 50;
    [SerializeField] int goldPenalty = 50;

    Coins coins;

    // Start is called before the first frame update
    void Start()
    {
        coins = FindObjectOfType<Coins>();
    }
    public void RewardCoins()
    {
        coins.Deposit(goldReward);
    }
    public void StealGold()
    {
        coins.WithDraw(goldPenalty);
    }
}
