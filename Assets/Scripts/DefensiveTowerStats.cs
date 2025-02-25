using UnityEngine;

[CreateAssetMenu(fileName = "DefensiveTower", menuName = "ScriptableObjects/DefensiveTower")]
public class DefensiveTowerStats : ScriptableObject
{
    public new string name;
    public GameObject prefab;
    public AttackType attackType;
    public string description;
    public int health = 10;
    public int attack = 0;
    public int attackDistance = 5;
    public int viewRange = 10;
    public int buildTime = 3;
    public int cost = 50;
    public Texture icon;
}

public enum AttackType
{
    Projectile,
    Melee,
    Support,
    Trap
}