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
    public string planetType = "";       // "gas"
    public float tempUpper = -1;       // 1000
    public float tempLower = -1;       // -1000
    public float lengthOfYear = -1;      // 100
    public int habitableZone = -1;       // 0 (e.g., index/flag)
    public string[] atmosphere= {""};       // "carbonDioxide"
    public float distanceFromStar=  -1;   // 100
}