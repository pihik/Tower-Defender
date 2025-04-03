using UnityEngine;

public class InGameHelper : MonoBehaviour
{
	public static InGameHelper instance;

	[Header("ObjectPool Storage")]
	[SerializeField] Transform defaultStorage;

	[Header("Layers")]
	[SerializeField] LayerMask playerLayer;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] LayerMask tileLayer;
	[SerializeField] LayerMask enviromentLayer;

	void Awake()
	{
		instance = this;
	}

	public LayerMask GetPlayerLayer()
	{
		return playerLayer;
	}

	public LayerMask GetEnemyLayer()
	{
		return enemyLayer;
	}

	public LayerMask GetTileLayer()
	{
		return tileLayer;
	}

	public LayerMask GetEnviromentLayer()
	{
		return enviromentLayer;
	}

	public Transform GetDefaultStorage()
	{
		return defaultStorage;
	}
}