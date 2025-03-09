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

        // probably not needed
        if (difficultyDropdown != null)
        {
            difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
        }
    }

    void InitializeDropdown()
    {
        List<string> options = new List<string> { "Easy", "Normal", "Hard" };

        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(options);

        difficultyDropdown.value = 1;
        difficultyDropdown.RefreshShownValue();
    }

    void OnDifficultyChanged(int value)
    {
        //selectedDifficulty = value;
    }

    public void ShowCurrentDifficulty()
    {
        difficultyDropdown.value = GameManager.instance.GetDifficulty();
        difficultyDropdown.RefreshShownValue();
    }

    public void Apply()
    {
        GameManager.instance.SetDifficulty(difficultyDropdown.value);
    }
}
