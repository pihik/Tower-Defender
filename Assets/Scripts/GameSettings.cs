using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	[SerializeField] TMP_Dropdown difficultyDropdown;

	/// <summary>
	/// 0 = Easy
	/// 1 = Normal
	/// 2 = Hard
	/// </summary>

	void Awake()
	{
		InitializeDropdown();
	}

	void InitializeDropdown()
	{
		List<string> options = new List<string> { "Easy", "Normal", "Hard" };

		difficultyDropdown.ClearOptions();
		difficultyDropdown.AddOptions(options);

		difficultyDropdown.value = GetDifficulty();
		difficultyDropdown.RefreshShownValue();
	}

	public void ShowCurrentDifficulty()
	{
		difficultyDropdown.value = GetDifficulty();
		difficultyDropdown.RefreshShownValue();
	}

	void SetDifficulty(int difficultyLevel)
	{
		PlayerPrefs.SetInt("GameDifficulty", difficultyLevel);
		PlayerPrefs.Save();
	}

	int GetDifficulty()
	{
		return PlayerPrefs.GetInt("GameDifficulty", 1);
	}

	public void Apply()
	{
		SetDifficulty(difficultyDropdown.value);
		GameManager.instance.SetDifficulty(difficultyDropdown.value);
	}
}
