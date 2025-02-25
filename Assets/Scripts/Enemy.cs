using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(Collider))]

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;

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
        currentHealth = enemyStats.health;
        enemyMover.SetMovementSpeed(enemyStats.movementSpeed);
    }

    public void RewardCoins()
    {
        shopManager.Deposit(enemyStats.coinReward);
    }

    public void StealGold()
    {
        shopManager.WithDraw(enemyStats.coinPenalty);
    }

    void OnParticleCollision(GameObject other)
    {
        currentHealth--;
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
