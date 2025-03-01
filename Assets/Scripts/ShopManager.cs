using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] int startingCoins = 100;
    [SerializeField] int currentCoins;
    [SerializeField] TextMeshProUGUI text;

    Tower SelectedTower;

    public int ActualCoins { get { return currentCoins; } }

    void Awake()
    {
        instance = this;
    }

    void Start()
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
            // maybe add additional ui logic or something
            LevelLoader.instance.ReloadScene();
        }
    }

    void UpdateGold() // maybe do this as action and put ui somewhere else on ui manager
    {
        text.text = "Gold: " + currentCoins.ToString();
    }

    public void SelectTower(Tower tower)
    {
        SelectedTower = tower;
    }

    public Tower GetSelectedTower()
    {
        return SelectedTower;
    }
}
