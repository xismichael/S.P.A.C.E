[System.Serializable]
public class Creature
{
    public int id;
    public string name;
    public string description;
    public CreatureTraits traits;
    public float size;
    public string origin;
}

[System.Serializable]
public class CreatureTraits
{
    public float vacuumResistance;
    public float radiationResistance;
    public float temperatureTolerance;
    public float oxygenRequirement;
    public float hydrationRequirement;
}