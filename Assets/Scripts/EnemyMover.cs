using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    List<Node> path = new List<Node>();
    Enemy enemy;
    PathFinding pathFinder;
    GridManager gridManager;

    private void OnEnable()
    {
        StartPosition();
        RecalculatePath(true);
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        pathFinder = PathFinding.instance;
        gridManager = GridManager.instance;
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(PrintWaypointName());
    }

    void StartPosition()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.StealGold();
        Destroy(gameObject);
    }

    IEnumerator PrintWaypointName()
    {
        for(int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float movePercentil = 0f;

            transform.LookAt(endPosition);

            while(movePercentil < 1f)
            {
                movePercentil += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, movePercentil);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }
}
