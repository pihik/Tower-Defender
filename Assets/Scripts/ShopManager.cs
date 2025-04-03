using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{
	public static ShopManager instance;

	public Action<int> OnGoldChanged;

	[SerializeField] int startingCoins = 100; // this value can be higher on higher difficulty
	[SerializeField] int currentCoins;

	Tower SelectedTower;

	public int ActualCoins { get { return currentCoins; } }

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		switch (GameManager.instance.GetDifficulty())
		{
			case 0:
				currentCoins = startingCoins;
				break;
			case 1:
				currentCoins = startingCoins * 2;
				break;
			case 2:
				currentCoins = startingCoins * 3;
				break;
			default:
				currentCoins = startingCoins;
				break;
		}

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
			GameManager.instance.OnLost?.Invoke();
		}
	}

	void UpdateGold()
	{
		OnGoldChanged?.Invoke(currentCoins);
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
