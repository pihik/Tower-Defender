using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Coins : MonoBehaviour
{
    [SerializeField] int startingCoins = 100;
    [SerializeField] int currentCoins;
    [SerializeField] TextMeshProUGUI text;
    public int ActualCoins { get { return currentCoins; } }

    private void Start()
    {
        currentCoins = startingCoins;
        UpdateGold();
    }

    public void Deposit(int amount)
    {
        currentCoins += Mathf.Abs(amount);
        UpdateGold();
    }
    public void WithDraw(int amount)
    {
        currentCoins -= Mathf.Abs(amount);
        UpdateGold();

        if (currentCoins < 0)
        {
            ReloadScene();
        }
    }

    void UpdateGold()
    {
        text.text = "Gold: " + currentCoins.ToString();
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

}
