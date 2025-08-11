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

    public List<Planet> GetUniquePlanets(int amount)
    {
        if (planetDict == null || planetDict.Count == 0)
            return new List<Planet>();

        int count = Mathf.Min(amount, planetDict.Count);
        var result = new List<Planet>();

        return GetUniquePlanetsRecursive(result, count);
    }

    private List<Planet> GetUniquePlanetsRecursive(List<Planet> current, int targetCount)
    {
        if (current.Count >= targetCount)
            return current;

        // Pick a random planet
        var allPlanets = planetDict.Values.ToList();
        Planet randomPlanet = allPlanets[UnityEngine.Random.Range(0, allPlanets.Count)];

        // Add if not already in the list
        if (!current.Contains(randomPlanet))
            current.Add(randomPlanet);

        // Recurse until we reach the desired amount
        return GetUniquePlanetsRecursive(current, targetCount);
    }
}