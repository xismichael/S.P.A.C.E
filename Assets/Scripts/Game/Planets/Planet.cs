using System;

[Serializable]
public class Planet
{
    public int id;
    public string name;
    public string description;
    public PlanetConditions conditions; // will be null if missing in JSON
    public string size; // nullable float
}

[Serializable]
public class PlanetConditions
{
    public string planetType;      // null if missing
    public string planetPrintType; // type to be printed out for player to see
    public float? tempUpper;       // null if missing
    public float? tempLower;
    public float? lengthOfYear;
    public int? habitableZone;
    public string[] atmosphere;    // null if missing
    public float? distanceFromStar;
}
