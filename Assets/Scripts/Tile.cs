using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceble;

    [SerializeField] Tower defensiveTower;

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
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            bool isSuccessfull = defensiveTower.CreateTower(defensiveTower, transform.position);
            if (isSuccessfull)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }
}
