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
<<<<<<< HEAD
    public float atmospherePressure;
    public float radiationLevel;
    public float averageTemperature;
    public float oxygenLevel;
    public float waterLevel;

    public void CreateConditions()
    {

    }
=======
    public string planetType;       // "gas"
    public float temperature;       // 1000
    public float lengthOfYear;      // 100
    public int habitableZone;       // 0 (e.g., index/flag)
    public string atmosphere;       // "carbonDioxide"
    public float distanceFromStar;   // 100
>>>>>>> 754eaa68ab866a50db8d7934d949e189f5cfd43b
}