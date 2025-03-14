using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(Collider))]

public class Enemy : MonoBehaviour
{
    Action OnStatsChanged;

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

        if (!enemyMover || !shopManager || !gameManager)
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
        OnStatsChanged?.Invoke();

        if (currentHealth <= 0)
        {
            RewardCoins();
            HandleDeath();
        }
    }
    
    void HandleDeath()
    {
        gameManager.OnEnemyDestroyed?.Invoke();
        SetFocus(false);
        Destroy(gameObject);
    }

    void PathFinished()
    {
        StealGold();
        gameManager.OnEnemyPathFinished?.Invoke();
        HandleDeath();
    }

    public void SetFocus(bool isFocused)
    {
        if (isFocused)
        {
            OnStatsChanged += UpdateUI;
            UpdateUI();
        }
        else
        {
            OnStatsChanged -= UpdateUI;
            UI_Manager.instance.ClearStatistics();
        }
    }

    void UpdateUI()
    {
        string healthText = stats.health.ToString() + "/" + currentHealth.ToString();
        UI_Manager.instance.SetStatistics(stats.name, stats.description, stats.attackType, healthText, stats.damage);
    }

    public EnemyStats GetStats()
    {
        return stats;
    }

    void OnDisable()
    {
        enemyMover.OnPathFinished -= PathFinished;
        OnStatsChanged -= UpdateUI;
    }
}
