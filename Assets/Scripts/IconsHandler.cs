using UnityEngine;
using UnityEngine.UI;

public class IconsHandler : MonoBehaviour
{
    Button[] icons;

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

        DeselectOthers(icon);
    }
}
