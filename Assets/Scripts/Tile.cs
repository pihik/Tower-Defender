using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceble;

    GridManager gridManager;
    PathFinding pathFinder;
    Vector2Int coordinates = new Vector2Int();

    /// <summary>
    /// TODO 
    /// - coop with shop
    /// - blocking tile if tree is there
    /// </summary>

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

            if(!isPlaceble)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        if (IsPointerOverUI())
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
