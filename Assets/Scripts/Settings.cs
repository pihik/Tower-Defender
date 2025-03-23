using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	[SerializeField] Button applyButton;

	AudioSettings audioSettings;
	GameSettings gameSettings;
	//GraphicsSettings graphicsSettings;
	//InputSettings inputSettings;

	void Awake()
	{
		audioSettings = GetComponent<AudioSettings>();
		gameSettings = GetComponent<GameSettings>();

		if (!applyButton || !audioSettings || !gameSettings)
		{
			Debug.Log("[Settings::Awake] Something went wrong.");
			return;
		}

		applyButton.onClick.AddListener(Apply);
	}

	public void Apply()
	{
		audioSettings.Apply();
		gameSettings.Apply();
		//graphicsSettings.Apply();
		//inputSettings.Apply();
	}
}
