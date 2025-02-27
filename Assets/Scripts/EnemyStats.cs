using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Attacker")]
public class EnemyStats : ScriptableObject
{
    public new string name;
    public GameObject Prefab;
    public AttackType attackType;
    public string description;
    public int health = 10;
    public float movementSpeed = 1f;
    public int attack = 1;
    public int attackDistance = 5;
    public int viewRange = 10;
    public int coinReward = 50;
    public int coinPenalty = 50;
}