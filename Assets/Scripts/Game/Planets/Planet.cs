using System;

[Serializable]
public class Planet
{
    public int id;
    public string name;
    public string description;
    public PlanetConditions conditions;
    public float size;
}

[Serializable]
public class PlanetConditions
{
    public string planetType;       // "gas"
    public float temperature;       // 1000
    public float lengthOfYear;      // 100
    public int habitableZone;       // 0 (e.g., index/flag)
    public string atmosphere;       // "carbonDioxide"
    public float distanceFromSun;   // 100
}