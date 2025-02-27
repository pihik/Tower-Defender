using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(Collider))]

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStats stats;

    EnemyMover enemyMover;
    ShopManager shopManager;

    int currentHealth = 5;

    void Awake()
    {
        enemyMover = GetComponent<EnemyMover>();
        shopManager = ShopManager.instance;

        if (!enemyMover || !shopManager)
        {
            Debug.LogError("[Enemy::Awake] Missing components");
        }

        enemyMover.OnPathFinished += PathFinished;
    }

    void Start()
    {
        InitializeStats();
    }

    void InitializeStats()
    {
        currentHealth = stats.health;
        enemyMover.SetStats(stats);
    }

    public void RewardCoins()
    {
        shopManager.Deposit(stats.coinReward);
    }

    public void StealGold()
    {
        shopManager.WithDraw(stats.coinPenalty);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }
    
    void HandleDeath()
    {
        RewardCoins();
        Destroy(gameObject);
    }

    void PathFinished()
    {
        StealGold();
        Destroy(gameObject);
    }

    void OnDisable()
    {
        enemyMover.OnPathFinished -= PathFinished;
    }
}
