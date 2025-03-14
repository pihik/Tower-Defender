using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconsHandler : MonoBehaviour
{
	Button[] icons;

	[SerializeField] TextMeshProUGUI popUpWindow;
	[SerializeField] Color selectedColor = Color.white;

	void Awake()
	{
		icons = GetComponentsInChildren<Button>();

		if (icons.Length == 0)
		{
			Debug.LogError("[IconsHandler::Awake] No icons found");
			return;
		}

		foreach (Button icon in icons)
		{
			InitializeColors(icon);
			icon.onClick.AddListener(() => OnIconClick(icon));
		}

		icons[0].onClick.Invoke();
	}

	void OnIconClick(Button icon)
	{
		SetButtonNormalColor(icon);
		DeselectOthers(icon);
	}

	void DeselectOthers(Button icon)
	{
		foreach (Button otherIcon in icons)
		{
			if (otherIcon != icon)
			{
				InitializeColors(otherIcon);
			}
		}
	}

	void InitializeColors(Button icon)
	{
		ColorBlock colorBlock = icon.colors;
		colorBlock.normalColor = Color.white;
		colorBlock.selectedColor = selectedColor;
		icon.colors = colorBlock;
	}

	void SetButtonNormalColor(Button icon)
	{
		ColorBlock colorBlock = icon.colors;
		colorBlock.normalColor = selectedColor;
		colorBlock.selectedColor = selectedColor;
		icon.colors = colorBlock;
	}

	public void ShowPopUp(DefenderStats stats)
	{
		bool hasStats = stats != null;
		popUpWindow.transform.parent.parent.gameObject.SetActive(hasStats);

		if (hasStats)
		{
			string popUpText = $"<b><size=21>{stats.name}</size></b>\n" +
							   $"<size=18>Cost: {stats.cost}</size>\n";
			popUpWindow.text = popUpText;
		}
	}

	public void SetPopUpLocationToTarget(GameObject target)
	{
		if (target == null)
		{
			Debug.LogError("[IconsHandler::SetPopUpLocationToTarget] Target is null");
			return;
		}

		RectTransform popUpRectTransform = popUpWindow.transform.parent.parent.GetComponent<RectTransform>();
		GridLayoutGroup gridLayoutGroup = target.GetComponentInParent<GridLayoutGroup>();

		if (!popUpRectTransform || !gridLayoutGroup)
		{
			Debug.LogError("[IconsHandler::SetPopUpLocationToTarget] Missing necessary components");
			return;
		}

		Vector2 cellSize = gridLayoutGroup.cellSize;
		Vector3 targetPosition = target.transform.position;
		Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + cellSize.y / 2 + popUpRectTransform.rect.height / 2, targetPosition.z);

		popUpRectTransform.position = newPosition;
	}
}
