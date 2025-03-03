using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isConstuctable = true;
    [SerializeField] bool pathBlocked;

    GridManager gridManager;
    PathFinding pathFinder;
    Vector2Int coordinates = new Vector2Int();

    /// <summary>
    /// TODO 
    /// - coop with shop
    /// - blocking tile if tree is there
    /// </summary>

    int tileLayerMask;

    void Awake()
    {
        gridManager = GridManager.instance;
        pathFinder = PathFinding.instance;
    }

    void Start()
    {
        if(gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(pathBlocked)
            {
                gridManager.BlockNode(coordinates);
            }
        }

        tileLayerMask = InGameHelper.instance.GetTileLayer();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.OnTileClicked();
                }
            }
        }
    }

    void OnTileClicked()
    {
        if (IsPointerOverUI() || !isConstuctable)
        {
            return;
        }

        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            Tower tower = ShopManager.instance.GetSelectedTower();

            if (!tower)
            {
                Debug.LogError("[Tile::OnMouseDown] Tower is missing");
                return;
            }

            bool isSuccessfull = tower.CreateTower(tower, transform.position);
            if (isSuccessfull)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }


    bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
