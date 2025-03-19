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
	int gridSize = 10;

	void Awake()
	{
		gridSize = GridManager.instance.UnityGridSize;

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
		coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridSize);
		coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridSize);

		textTMP.text = coordinates.x + "," + coordinates.y;
	}
	void UpdateTileName()
	{
		transform.parent.name = coordinates.ToString();
	}
}
