using JetBrains.Annotations;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;


    // TODO updating health on widget and update widget only if it's clicked

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public Action<int> OnDifficultyChanged;
    public Action<int> OnHealthChanged;
    public Action OnEnemyPathFinished;
    public Action OnEnemyDestroyed;
    public Action OnWin;
    public Action OnLost;

    int amountOfEnemies = int.MaxValue;
    int health = 3;

    int difficulty = 1;

    void OnEnable()
    {
        OnEnemyDestroyed += DecreaseAmountOfEnemies;
        OnEnemyPathFinished += EnemyFinishedPath;
    }

    public void SetNumberOfEnemies(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("[GameManager::SetNumberOfEnemies] Invalid amount");
            return;
        }

        amountOfEnemies = amount;
    }

    void DecreaseAmountOfEnemies()
    {
        amountOfEnemies--;

        if (amountOfEnemies <= 0)
        {
            OnWin?.Invoke();
        }
    }

    void EnemyFinishedPath()
    {
        health--;
        if (health <= 0)
        {
            OnLost?.Invoke();
            return;
        }

        OnHealthChanged?.Invoke(health);
    }

    void OnDisable()
    {
        OnEnemyDestroyed -= DecreaseAmountOfEnemies;
        OnEnemyPathFinished -= EnemyFinishedPath;
    }

    public void SetDifficulty(int value)
    {
        Debug.Log("Difficulty changed to: " + value);
        difficulty = value;
        OnDifficultyChanged?.Invoke(difficulty);
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
