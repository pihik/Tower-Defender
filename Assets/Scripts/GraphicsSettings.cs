using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
	[SerializeField] TMP_Dropdown qualityDropdown;
	[SerializeField] TMP_Dropdown vSyncDropdown;
	[SerializeField] TMP_Dropdown antiAliasingDropdown;
	[SerializeField] TMP_Dropdown shadowsDropdown;

	void Start()
	{
		InitializeDropdowns();
		LoadSettings();
	}

	void InitializeDropdowns()
	{
		InitializeQualityDropdown();
		InitializeVSyncDropdown();
		InitializeAntiAliasingDropdown();
		InitializeShadowsDropdown();
	}

	void InitializeQualityDropdown()
	{
		List<string> options = new List<string>(QualitySettings.names);
		qualityDropdown.ClearOptions();
		qualityDropdown.AddOptions(options);
	}

	void InitializeVSyncDropdown()
	{
		List<string> options = new List<string> { "Off", "On" };
		vSyncDropdown.ClearOptions();
		vSyncDropdown.AddOptions(options);
	}

	void InitializeAntiAliasingDropdown()
	{
		List<string> options = new List<string> { "Off", "2x MSAA", "4x MSAA", "8x MSAA" };
		antiAliasingDropdown.ClearOptions();
		antiAliasingDropdown.AddOptions(options);
	}

	void InitializeShadowsDropdown()
	{
		List<string> options = new List<string> { "Disabled", "Hard Shadows", "Soft Shadows" };
		shadowsDropdown.ClearOptions();
		shadowsDropdown.AddOptions(options);
	}

	void LoadSettings()
	{
		qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
		vSyncDropdown.value = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount);
		antiAliasingDropdown.value = PlayerPrefs.GetInt("AntiAliasing", Mathf.Clamp(QualitySettings.antiAliasing / 2, 0, 3));
		shadowsDropdown.value = PlayerPrefs.GetInt("Shadows", (int)QualitySettings.shadows);

		QualitySettings.SetQualityLevel(qualityDropdown.value);
		QualitySettings.vSyncCount = vSyncDropdown.value;
		QualitySettings.antiAliasing = antiAliasingDropdown.value * 2;
		QualitySettings.shadows = (ShadowQuality)shadowsDropdown.value;

		Application.targetFrameRate = (vSyncDropdown.value == 0) ? -1 : 144;
	}

	void SaveSettings()
	{
		PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
		PlayerPrefs.SetInt("VSync", vSyncDropdown.value);
		PlayerPrefs.SetInt("AntiAliasing", antiAliasingDropdown.value);
		PlayerPrefs.SetInt("Shadows", shadowsDropdown.value);
		PlayerPrefs.Save();
	}

	public void Apply()
	{
		SaveSettings();

		QualitySettings.SetQualityLevel(qualityDropdown.value);
		QualitySettings.vSyncCount = vSyncDropdown.value;
		QualitySettings.antiAliasing = antiAliasingDropdown.value * 2;
		QualitySettings.shadows = (ShadowQuality)shadowsDropdown.value;

		Application.targetFrameRate = (vSyncDropdown.value == 0) ? -1 : 144;
	}
}
