using UnityEngine;

[CreateAssetMenu(fileName = "DefensiveTower", menuName = "ScriptableObjects/Tower")]
public class DefenderStats : DefaultStats
{
    public int buildTime = 3;
    public Texture icon;
    public int cost = 50;
}