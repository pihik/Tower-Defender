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
    GameManager gameManager;

    int currentHealth = 5;

    void Awake()
    {
        enemyMover = GetComponent<EnemyMover>();
        shopManager = ShopManager.instance;
        gameManager = GameManager.instance;

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

    void RewardCoins()
    {
        shopManager.Deposit(stats.coinReward);
    }

    void StealGold()
    {
        shopManager.WithDraw(stats.coinPenalty);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            RewardCoins();
            HandleDeath();
        }
    }
    
    void HandleDeath()
    {
        gameManager.OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }

    void PathFinished()
    {
        StealGold();
        gameManager.OnEnemyPathFinished?.Invoke();
        HandleDeath();
    }

    void OnDisable()
    {
        enemyMover.OnPathFinished -= PathFinished;
    }

    public EnemyStats GetStats()
    {
        return stats;
    }
}
