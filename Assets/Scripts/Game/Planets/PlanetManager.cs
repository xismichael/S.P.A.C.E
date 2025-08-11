using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] private RectTransform windowContainer;
    [SerializeField] private PlanetInfoView planetInfoView;
    [SerializeField] private GameObject planet_prefab;
    [SerializeField] private Image SelectedPlanetImage;
    public static PlanetUI selectedPlanetUI;
    public static Planet SelectedPlanet;

    public int PlanetCount = 5;
    public List<PlanetUI> CurrentPlanets = new();

    public List<Planet> planets;

    // Edit in Inspector if you want; these are pixels in windowContainer's anchored space
    [SerializeField]
    private List<Vector2> planetPositions = new()
    {
        new Vector2(-400, 225),
        new Vector2(150, 200),
        new Vector2(-50, -20),
        new Vector2(-250, -200),
        new Vector2(300, -150)
    };

    public void startGame()
    {
        if (!planet_prefab || !windowContainer)
        {
            Debug.LogError("[PlanetManager] Missing prefab or windowContainer reference.", this);
            return;
        }

        // Get data
        planets = PlanetDatabase.Instance.GetUniquePlanets(PlanetCount);

        // How many can we actually place without repeating a slot?
        int spawnCount = Mathf.Min(planets.Count, planetPositions.Count);
        if (planets.Count > planetPositions.Count)
            Debug.LogWarning($"Requested {planets.Count} planets but only {planetPositions.Count} positions. Spawning {spawnCount}.");

        // Clear any old UI (optional)
        foreach (var ui in CurrentPlanets) if (ui) Destroy(ui.gameObject);
        CurrentPlanets.Clear();

        // Sequentially assign positions
        for (int i = 0; i < spawnCount; i++)
        {
            var go = Instantiate(planet_prefab, windowContainer);
            var rt = go.GetComponent<RectTransform>();
            if (rt)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = planetPositions[i]; // unique slot, no repeats
            }

            var ui = go.GetComponent<PlanetUI>();
            ui.planet = planets[i];
            ui.planetinfoview = planetInfoView;
            ui.planetManager = this; // Set the manager reference
            // ui.manager = this; // if your PlanetUI needs a back-reference
            ui.Initialize();

            CurrentPlanets.Add(ui);
        }
    }

    public void SetPlanetUI(PlanetUI planetUI)
    {
        selectedPlanetUI = planetUI;
        SelectedPlanet = planetUI.planet;
        SelectedPlanetImage = planetUI.GetComponent<Image>();
    }


}




    // void CreatePlanets()
    // {
    //     planet_values = PlanetDatabase.Instance.GetAllPlanets();

    //     foreach (var planet_value in planet_values)
    //     {
    //         GameObject planet = Instantiate(planet_prefab, new Vector3(0, 0, 0), Quaternion.identity);
    //         planet.GetComponent<PlanetUI>().planetinfoview = GameObject.Find("PlanetInfoView");
    //         planets.Add(planet);

    //     }
    // }

