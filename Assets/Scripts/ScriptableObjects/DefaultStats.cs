using UnityEngine;

public abstract class DefaultStats : ScriptableObject
{
	public new string name;
	public AttackType attackType;
	public string description;
	public int health = 10;
	public int damage = 1;
	public float attackSpeed = 1;
}

public enum AttackType
{
	Melee,
	Ranged,
	Trap
}
