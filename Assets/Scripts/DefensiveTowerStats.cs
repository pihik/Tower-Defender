using UnityEngine;

[CreateAssetMenu(fileName = "DefensiveTower", menuName = "ScriptableObjects/DefensiveTower")]
public class DefensiveTowerStats : ScriptableObject
{
    public new string name;
    public GameObject Prefab;
    public DefensiveTowerType TowerType;
    public string description;
    public int health = 10;
    public int attack = 0;
    public int viewRange = 10;
    public int buildTime = 3;
    public int cost = 50;
    public Texture icon;
}

public enum DefensiveTowerType
{
    Projectile,
    Melee,
    Support,
    Trap
}