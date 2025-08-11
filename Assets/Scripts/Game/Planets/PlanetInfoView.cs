using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetInfoView : MonoBehaviour
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

    public void Open(Planet planet)
    {

        info.text = $"Planet Name: {planet.name}\n" +
                    $"Description: {planet.description}\n" +
                    $"Size: {planet.size}\n";

        if (planet.conditions != null)
        {
            if (planet.conditions.atmosphere != null) info.text += $"Atmosphere: {string.Join(", ", planet.conditions.atmosphere)}\n";
            if (planet.conditions.distanceFromStar != null) info.text += $"Distance from Star: {planet.conditions.distanceFromStar} AU\n";
            if (planet.conditions.planetType != null) info.text += $"Planet Type: {planet.conditions.planetType}\n";
            if (planet.conditions.tempLower != null && planet.conditions.tempUpper != null) info.text +=$"Temperature Range: {planet.conditions.tempLower}°C to {planet.conditions.tempUpper}°C\n";
            if (planet.conditions.lengthOfYear != null) info.text +=$"Length of Year: {planet.conditions.lengthOfYear} Earth days\n";
            if (planet.conditions.habitableZone != null) info.text +=$"Habitable Zone: {planet.conditions.habitableZone}\n";
            if (planet.conditions.atmosphere != null) info.text +=$"Atmosphere: {string.Join(", ", planet.conditions.atmosphere)}";
        }
        gameObject.SetActive(true);
    }


}
