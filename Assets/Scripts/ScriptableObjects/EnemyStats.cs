using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Attackers/DefaultEnemy")]
public class EnemyStats : DefaultStats
{
	public float movementSpeed = 1f;
	public int coinReward = 50;
	public int coinPenalty = 50;
}