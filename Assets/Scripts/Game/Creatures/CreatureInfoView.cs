using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class CreatureInfoView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private TextMeshProUGUI info;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }

    public void Open(Creature creature)
    {

        info.text = $"Creature Name: {creature.name}\n" +
                    $"Description: {creature.description}\n" +
                    $"Size: {creature.size}\n" +
                    $"Temperature Range: {creature.traits.tempLower}°C to {creature.traits.tempUpper}°C\n" +
                    $"Atmosphere can survive in: {string.Join(", ", creature.traits.atmosphere)}" +
                    $"Raidiation Range: {creature.traits.radiationLower}units to {creature.traits.radiationUpper}units\n" +
                    $"Biomes: {string.Join(", ", creature.traits.biome)}\n" +
                    $"Lifespan: {creature.traits.lifeSpan} years\n" +
                    $"General Resilience: {creature.traits.generalResilience}\n" +
                    $"has Beef with: {string.Join(", ", creature.traits.beef)}\n" +
                    $"starting sanity on the ship: {creature.traits.sanity}\n";
        gameObject.SetActive(true);
    }

    
}