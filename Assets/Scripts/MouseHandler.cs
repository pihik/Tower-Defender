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

    int playerLayer;
    int enemyLayer;
    int enviromentLayer;

    void Start()
    {
        playerLayer = InGameHelper.instance.GetPlayerLayer();
        enemyLayer = InGameHelper.instance.GetEnemyLayer();
        enviromentLayer = InGameHelper.instance.GetEnviromentLayer();
    }

    void Update()
    {
        HandleCursorTexture();
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
}
