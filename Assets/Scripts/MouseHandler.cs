using System.Security.Cryptography;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] Texture2D mainCursorTexture;
    [SerializeField] Texture2D playerCursor;
    [SerializeField] Texture2D enemyCursor;
    [SerializeField] Texture2D enviromentCursor;

    [Header("Cursor Hotspot")]
    [SerializeField] Vector2 cursorHotspot;

    [Header("Selection")]
    [SerializeField] GameObject selection;
    [SerializeField] LineRenderer lineRenderer;

	int playerLayer, enemyLayer, enviromentLayer, tileLayer;

	GameObject target;

    void Start()
    {
        playerLayer = InGameHelper.instance.GetPlayerLayer();
        enemyLayer = InGameHelper.instance.GetEnemyLayer();
        enviromentLayer = InGameHelper.instance.GetEnviromentLayer();
        tileLayer = InGameHelper.instance.GetTileLayer();

        InitializeLineRenderer();
    }

    void Update()
    {
        HandleCursorTexture();
        OnLeftClick();
        MoveSelection();
    }

    void InitializeLineRenderer()
    {
        lineRenderer.positionCount = 100 + 1;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void HandleCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
			switch (hit.collider.gameObject.layer)
			{
				case var layer when (playerLayer & (1 << layer)) != 0:
					Cursor.SetCursor(playerCursor, cursorHotspot, CursorMode.Auto);
					break;

				case var layer when (enemyLayer & (1 << layer)) != 0:
					Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
					break;

				case var layer when (enviromentLayer & (1 << layer)) != 0:
					Cursor.SetCursor(enviromentCursor, cursorHotspot, CursorMode.Auto);
					break;

				default:
					Cursor.SetCursor(mainCursorTexture, cursorHotspot, CursorMode.Auto);
					break;
			}
		}
        else
        {
            Cursor.SetCursor(mainCursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }

    void OnLeftClick()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            SelectionToggle(false);
            lineRenderer.enabled = false;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
				GameObject clickedObject = hit.collider.gameObject;

				if ((playerLayer & (1 << clickedObject.layer)) != 0)
				{
					HandleClickedOnStatsObject(clickedObject);
					if (clickedObject.TryGetComponent(out SphereCollider rangeComponent))
					{
						DrawCircle(clickedObject.transform.position, rangeComponent.radius);
					}
				}
				else if ((enemyLayer & (1 << clickedObject.layer)) != 0)
				{
					HandleClickedOnStatsObject(clickedObject);
				}
				else if ((tileLayer & (1 << clickedObject.layer)) != 0)
				{
					clickedObject.GetComponent<Tile>()?.OnTileClicked();
				}
			}
        }
    }

    bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void HandleClickedOnStatsObject(GameObject statsObject)
    {
		if (statsObject.TryGetComponent(out Tower player))
		{
			ClearFocused();
			SetStatistics(player.GetStats());
			SetTarget(statsObject);
		}
		else if (statsObject.TryGetComponent(out Enemy enemy))
		{
			ClearFocused();
			enemy.SetFocus(true);
			SetTarget(statsObject);
		}
	}

    void ClearFocused()
    {
		if (target && target.TryGetComponent(out Enemy enemy))
		{
			enemy.SetFocus(false);
		}
	}

    void SetStatistics(DefaultStats stats)
    {
        if (!stats)
        {
            return;
        }

        UI_Manager.instance.SetStatistics(stats.name, stats.description, stats.attackType, stats.health.ToString(), stats.damage);
    }

    void SelectionToggle(bool activate)
    {
        if (selection.activeSelf && (!activate || !target))
        {
            selection.SetActive(false);
            return;
        }

        selection.SetActive(true);
    }

    void MoveSelection()
    {
        if (!target)
        {
            selection.SetActive(false);
            return;
        }

        if (selection.activeSelf)
        {
            selection.transform.position = target.transform.position;
        }
    }

    void SetTarget(GameObject target)
    {
        this.target = target;
		SelectionToggle(true);
	}

    void DrawCircle(Vector3 position, float viewRange)
    {
        if (lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
        }

        float angleStep = 360f / 100;
        for (int i = 0; i < 100 + 1; i++)
        {
            float angle = i * angleStep;
            float x = position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * viewRange;
            float z = position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * viewRange;
            lineRenderer.SetPosition(i, new Vector3(x, 1, z));
        }
    }
}
