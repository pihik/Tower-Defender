using UnityEngine;

public class InGameHelper : MonoBehaviour
{
    public static InGameHelper instance;

    [Header("ObjectPool Storage")]
    [SerializeField] Transform defaultStorage;
    [SerializeField] Transform projectileStorage;

    [Header("Layers")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask tileLayer;

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

    public Transform GetDefaultStorage()
    {
        return defaultStorage;
    }

    public Transform GetProjectileStorage()
    {
        return projectileStorage;
    }
}