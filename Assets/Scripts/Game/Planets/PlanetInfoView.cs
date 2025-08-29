using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetInfoView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI description;
    public TextMeshProUGUI planetType;
    public TextMeshProUGUI tempRange;
    public TextMeshProUGUI lengthOfYear;
    public TextMeshProUGUI atmosphere;
    public TextMeshProUGUI distanceFromStar;
    public TextMeshProUGUI habitableZone;
    public TextMeshProUGUI size;
    public Animator planetAnimator;
    public GameObject disclosureScreen;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {

        //play button close sound
        SoundManager.Instance.PlayClick(2);
        
        gameObject.SetActive(false);
    }

    public void Open(Planet planet)
    {

        //play button open sound
        SoundManager.Instance.PlayClick(1);

        description.text = planet.description;


        if (planet.conditions != null)
        {
            if (planet.conditions.atmosphere != null && planet.conditions.atmosphere.Length > 0)
            {
                atmosphere.text = string.Join(", ", planet.conditions.atmosphere);
            }
            else
            {
                atmosphere.text = "Data Unavailable";
            }

            if (planet.conditions.distanceFromStar != null) distanceFromStar.text = $"{planet.conditions.distanceFromStar}AU";
            else distanceFromStar.text = "Data Unavailable";

            if (planet.conditions.planetType != null) planetType.text = planet.conditions.planetPrintType;
            else planetType.text = "Data Unavailable";

            if (planet.conditions.tempLower != null && planet.conditions.tempUpper != null)
            {
                if (planet.conditions.tempLower == planet.conditions.tempUpper)
                {
                    tempRange.text = $"around {planet.conditions.tempLower}K";
                }
                else
                {
                    tempRange.text = $"{planet.conditions.tempLower}K to {planet.conditions.tempUpper}K";
                }
            }
            else tempRange.text = "Data Unavailable";

            if (planet.conditions.lengthOfYear != null) lengthOfYear.text = $"{planet.conditions.lengthOfYear} Earth days";
            else lengthOfYear.text = "Data Unavailable";

            if (planet.conditions.habitableZone != null)
            {
                if (planet.conditions.habitableZone == 1) habitableZone.text = "Within System's Habitable Zone\n";
                else habitableZone.text = "Not Within System's Habitable Zone\n";
            }
            else habitableZone.text = "Data Unavailable\n";
        }


            if (planet.size != null) size.text = planet.size;
            else size.text = "Data Unavailable";

        gameObject.SetActive(true);
        planetAnimator.Play(planet.name);
    }

    public void UpdateInfoView(Planet planet)
    {
        //check if the gameobject is active
        if (!gameObject.activeSelf) return;


        description.text = planet.description;


        if (planet.conditions != null)
        {
            if (planet.conditions.atmosphere != null && planet.conditions.atmosphere.Length > 0)
            {
                atmosphere.text = string.Join(", ", planet.conditions.atmosphere);
            }
            else
            {
                atmosphere.text = "Data Unavailable";
            }

            if (planet.conditions.distanceFromStar != null) distanceFromStar.text = $"{planet.conditions.distanceFromStar}AU";
            else distanceFromStar.text = "Data Unavailable";

            if (planet.conditions.planetType != null) planetType.text = planet.conditions.planetPrintType;
            else planetType.text = "Data Unavailable";

            if (planet.conditions.tempLower != null && planet.conditions.tempUpper != null)
            {
                if (planet.conditions.tempLower == planet.conditions.tempUpper)
                {
                    tempRange.text = $"around {planet.conditions.tempLower}K";
                }
                else
                {
                    tempRange.text = $"{planet.conditions.tempLower}K to {planet.conditions.tempUpper}K";
                }
            }
            else tempRange.text = "Data Unavailable";

            if (planet.conditions.lengthOfYear != null) lengthOfYear.text = $"{planet.conditions.lengthOfYear} Earth days";
            else lengthOfYear.text = "Data Unavailable";

            if (planet.conditions.habitableZone != null)
            {
                if (planet.conditions.habitableZone == 1) habitableZone.text = "Within System's Habitable Zone\n";
                else habitableZone.text = "Not Within System's Habitable Zone\n";
            }
            else habitableZone.text = "Data Unavailable\n";

            if (planet.size != null) size.text = planet.size;
            else size.text = "Data Unavailable";
        }
        planetAnimator.Play(planet.name);


    }

    public void OnDisclosureButton()
    {

        //play button open sound
        SoundManager.Instance.PlayClick(1);


        disclosureScreen.SetActive(true);
    }

    public void OnCloseDisclosure()
    {
        //play button close sound
        SoundManager.Instance.PlayClick(2);

        disclosureScreen.SetActive(false);
    }


}
