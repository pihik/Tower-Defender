using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	public Action OnPathFinished;

	List<Node> path = new List<Node>();

	PathFinding pathFinder;
	GridManager gridManager;

	EnemyStats stats;
	
	void Awake()
	{
		pathFinder = PathFinding.instance;
		gridManager = GridManager.instance;

		if (!pathFinder || !gridManager)
		{
			Debug.LogError("[EnemyMover::Awake] Missing components");
		}
	}

	void InitializeMySelf()
	{
		StartPosition();
		RecalculatePath(true);
	}

	public void SetStats(EnemyStats stats)
	{
		this.stats = stats;

		InitializeMySelf();
	}

	void RecalculatePath(bool resetPath)
	{
		Vector2Int coordinates = new Vector2Int();

		coordinates = (resetPath) ? pathFinder.StartCoordinates : gridManager.GetCoordinatesFromPosition(transform.position);

		StopAllCoroutines();
		path.Clear();
		path = pathFinder.GetNewPath(coordinates);

		StartCoroutine(MoveAlongPath());
	}

	void StartPosition()
	{
		transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
	}

	IEnumerator MoveAlongPath()
	{
		for(int i = 1; i < path.Count; i++)
		{
			Vector3 startPosition = transform.position;
			Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
			float movePercentil = 0f;

			transform.LookAt(endPosition);

			while(movePercentil < 1f)
			{
				if (!stats)
				{
					Debug.LogError("[EnemyMover::MoveAlongPath] Missing stats");
					yield break;
				}

				movePercentil += Time.deltaTime * stats.movementSpeed;
				transform.position = Vector3.Lerp(startPosition, endPosition, movePercentil);
				yield return new WaitForEndOfFrame();
			}
		}

		OnPathFinished?.Invoke();
	}
}
