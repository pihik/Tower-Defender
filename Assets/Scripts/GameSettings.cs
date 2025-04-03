using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	[SerializeField] TMP_Dropdown difficultyDropdown;
	[SerializeField] CustomChecker fpsChecker;

	/// <summary>
	/// 0 = Easy
	/// 1 = Normal
	/// 2 = Hard
	/// </summary>

	void Start()
	{
		InitializeDropdown();
		fpsChecker.InitializeCheckerValue(GetCanShowFPS());
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

	public void SaveShowFPS(bool show)
	{
		PlayerPrefs.SetInt("ShowFPS", show ? 1 : 0);
		PlayerPrefs.Save();
	}

	bool GetCanShowFPS()
	{
		return PlayerPrefs.GetInt("ShowFPS", 0) == 1;
	}

	public void Apply()
	{
		SetDifficulty(difficultyDropdown.value);
		SaveShowFPS(fpsChecker.IsChecked());
		GameManager.instance.SetDifficulty(difficultyDropdown.value);
		FPSChecker.instance.TextSwitch(fpsChecker.IsChecked());
	}
}
