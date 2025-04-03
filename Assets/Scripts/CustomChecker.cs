using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomChecker : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] bool isChecked;
	[SerializeField] Texture checkedImage;
	[SerializeField] Texture uncheckedImage;

	RawImage graphicsImage;

	void Awake()
	{
		graphicsImage = GetComponentInChildren<RawImage>();
	}
	void Start()
	{
		//graphicsImage.texture = null;
	}

	public void InitializeCheckerValue(bool value)
	{
		isChecked = value;
		graphicsImage.texture = (isChecked) ? checkedImage : uncheckedImage;
		FPSChecker.instance.TextSwitch(isChecked);
	}

	public void ToggleCheck()
	{
		isChecked = !isChecked;
		graphicsImage.texture = (isChecked) ? checkedImage : uncheckedImage;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ToggleCheck();
	}

	public bool IsChecked()
	{
		return isChecked;
	}
}