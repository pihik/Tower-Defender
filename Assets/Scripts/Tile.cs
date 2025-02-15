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

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinding>();
    }

    private void Start()
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

    private void OnMouseDown()
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
