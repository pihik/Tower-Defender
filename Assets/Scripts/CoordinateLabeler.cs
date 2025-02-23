using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]

public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro textTMP;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake()
    {
        gridManager = GridManager.instance;
        textTMP = GetComponent<TextMeshPro>();
        DisplayCoordinates();
    }

    void Update()
    {
        if(!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateTileName();
        }
        CoordinatesVisibilitySwitch();
    }

    void CoordinatesVisibilitySwitch()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            textTMP.enabled = !textTMP.IsActive();
        }
    }

    void DisplayCoordinates()
    {
        if(gridManager == null) { return; }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        textTMP.text = coordinates.x + "," + coordinates.y;
    }
    void UpdateTileName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
