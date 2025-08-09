using System;

[Serializable]
public class Creature
{
    public int id;
    public string name;
    public string description;
    public CreatureTraits traits;
    public float size;
    public string origin;
}

[Serializable]
public class CreatureTraits
{
    // Matches corrected JSON keys exactly
    public float tempUpper;
    public float tempLower;
    public string[] atmosphere;      // e.g., ["oxygen", "carbonDioxide"]
    public float radiationUpper;
    public float radiationLower;
    public string[] biome;           // e.g., ["ocean", "rock", "gas"]
    public int lifeSpan;             // 100
    public float generalResilience;  // 0
}
