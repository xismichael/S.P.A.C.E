[System.Serializable]
public class Planet
{
    public int id;
    public string name;
    public string description;
    public PlanetConditions conditions;
    public float gravity;
    public float distanceFromEarth;
}

[System.Serializable]
public class PlanetConditions
{
    public float atmospherePressure;
    public float radiationLevel;
    public float averageTemperature;
    public float oxygenLevel;
    public float waterLevel;

    public void CreateConditions()
    {

    }
}