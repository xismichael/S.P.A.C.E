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
}