using UnityEngine;

public class InGameHelper : MonoBehaviour
{
    public static InGameHelper instance;

    [Header("Layers")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

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
}