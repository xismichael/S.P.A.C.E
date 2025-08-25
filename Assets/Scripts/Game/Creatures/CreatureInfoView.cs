using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class CreatureInfoView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private TextMeshProUGUI info;
    public Animator creatureAnimator;
    public TextMeshProUGUI description;
    public TextMeshProUGUI thermalTolerance;
    public TextMeshProUGUI respiratoryMedium;
    public TextMeshProUGUI radiationTolerance;
    public TextMeshProUGUI ecologicalAffinities;
    public TextMeshProUGUI longevityIndex;
    public TextMeshProUGUI InterSpeciesHostility;

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

    public void UpdateInfoView(Creature creature)
    {
        //check if the gameobject is active
        if (!gameObject.activeSelf) return;

        description.text = creature.description;
        thermalTolerance.text = creature.ambiguousData.thermalTolerance;
        respiratoryMedium.text = creature.ambiguousData.respiratoryMedium;
        radiationTolerance.text = creature.ambiguousData.radiationTolerance;
        ecologicalAffinities.text = creature.ambiguousData.ecologicalAffinities;
        longevityIndex.text = creature.ambiguousData.longevityIndex;
        InterSpeciesHostility.text = creature.ambiguousData.InterSpeciesHostility;
        creatureAnimator.Play(creature.name);
    }

    public void Open(Creature creature)
    {


        description.text = creature.description;
        thermalTolerance.text = creature.ambiguousData.thermalTolerance;
        respiratoryMedium.text = creature.ambiguousData.respiratoryMedium;
        radiationTolerance.text = creature.ambiguousData.radiationTolerance;
        ecologicalAffinities.text = creature.ambiguousData.ecologicalAffinities;
        longevityIndex.text = creature.ambiguousData.longevityIndex;
        InterSpeciesHostility.text = creature.ambiguousData.InterSpeciesHostility;

        // info.text = $"Creature Name: {creature.name}\n" +
        //             $"Description: {creature.description}\n" +
        //             $"Size: {creature.size}\n" +
        //             $"Temperature Range: {creature.traits.tempLower}°C to {creature.traits.tempUpper}°C\n" +
        //             $"Atmosphere can survive in: {string.Join(", ", creature.traits.atmosphere)}" +
        //             $"Raidiation Range: {creature.traits.radiationLower}units to {creature.traits.radiationUpper}units\n" +
        //             $"Biomes: {string.Join(", ", creature.traits.biome)}\n" +
        //             $"Lifespan: {creature.traits.lifeSpan} years\n" +
        //             $"General Resilience: {creature.traits.generalResilience}\n" +
        //             $"has Beef with: {string.Join(", ", creature.traits.beef)}\n" +
        //             $"starting sanity on the ship: {creature.traits.sanity}\n";
        gameObject.SetActive(true);
        creatureAnimator.Play(creature.name);
    }

    
}