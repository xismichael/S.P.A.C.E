using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class CreatureDatabase : MonoBehaviour
{
    private static CreatureDatabase instance;
    public static CreatureDatabase Instance => instance;

    private Dictionary<string, Creature> creatureDict;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadCreatures();
        }
    }

    private void LoadCreatures()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("creatures");
        List<Creature> creatureList = JsonConvert.DeserializeObject<List<Creature>>(jsonFile.text);
        creatureDict = creatureList.ToDictionary(c => c.name);
        //Debug.Log($"Loaded {creatureDict.Count} creatures from database.");
    }

    public Creature GetCreature(string name)
    {
        return creatureDict.ContainsKey(name) ? creatureDict[name] : null;
    }

    public IEnumerable<Creature> GetAllCreatures()
    {
        return creatureDict.Values;
    }

    public List<Creature> GetUniqueCreatures(int amount)
    {
        if (creatureDict == null || creatureDict.Count == 0)
            return new List<Creature>();

        int count = Mathf.Min(amount, creatureDict.Count);
        var result = new List<Creature>();

        return GetUniqueCreaturesRecursive(result, count);
    }

    private List<Creature> GetUniqueCreaturesRecursive(List<Creature> current, int targetCount)
    {
        if (current.Count >= targetCount)
            return current;

        // Pick a random planet
        var allCreatures = creatureDict.Values.ToList();
        Creature randomCreature = allCreatures[UnityEngine.Random.Range(0, allCreatures.Count)];

        // Add if not already in the list
        if (!current.Contains(randomCreature))
            current.Add(randomCreature);

        // Recurse until we reach the desired amount
        return GetUniqueCreaturesRecursive(current, targetCount);
    }
}