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

    int playerLayer;
    int enemyLayer;
    int enviromentLayer;
    int tileLayer;

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
            if ((playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Cursor.SetCursor(playerCursor, cursorHotspot, CursorMode.Auto);
            }
            else if ((enemyLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
            }
            else if ((enviromentLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Cursor.SetCursor(enviromentCursor, cursorHotspot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(mainCursorTexture, cursorHotspot, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(mainCursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }

    void OnLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            SelectionTogge(false);
            lineRenderer.enabled = false;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if ((playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    if (hit.collider.gameObject.TryGetComponent(out SphereCollider rangeComponent))
                    {
                        HandleClickedOnStatsObject(hit.collider.gameObject);
                        DrawCircle(hit.collider.gameObject.transform.position, rangeComponent.radius);
                        SetTarget(hit.collider.gameObject);
                        SelectionTogge(true);
                    }
                }
                else if ((enemyLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    HandleClickedOnStatsObject(hit.collider.gameObject);
                    SetTarget(hit.collider.gameObject);
                    SelectionTogge(true);
                }
                else if ((tileLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                    {
                        tile.OnTileClicked();
                    }
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
        }
        else if (statsObject.TryGetComponent(out Enemy enemy))
        {
            ClearFocused();
            enemy.SetFocus(true);
            //stats = enemy.GetStats();
        }
    }

    void ClearFocused()
    {
        if (target)
        {
            if (target.TryGetComponent(out Enemy enemy))
            {
                enemy.SetFocus(false);
            }
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

    void SelectionTogge(bool activate)
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
