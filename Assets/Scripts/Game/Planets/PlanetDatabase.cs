using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class PlanetDatabase : MonoBehaviour
{
    private static PlanetDatabase instance;
    public static PlanetDatabase Instance => instance;

    private Dictionary<string, Planet> planetDict;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadPlanets();
        }
    }

    private void LoadPlanets()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("planets");
        List<Planet> planetList = JsonConvert.DeserializeObject<List<Planet>>(jsonFile.text);
        planetDict = planetList.ToDictionary(c => c.name);
    }

    public Planet GetPlanet(string name)
    {
        return planetDict.ContainsKey(name) ? planetDict[name] : null;
    }

    public IEnumerable<Planet> GetAllPlanets()
    {
        return planetDict.Values;
    }
}