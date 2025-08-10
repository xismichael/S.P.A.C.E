using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public SpaceZoom spaceZoom;
    private PlanetUI selectedPlanetUI;
    private Planet SelectedPlanet;
    private bool isPlanetSelected;
    //public PlanetInfoView planetInfoView;

    //public GameObject planet_prefab;
    //public List<GameObject> planets;

    IEnumerable<Planet> planet_values;

    void Start()
    {
        //CreatePlanets();
    }

    // Update is called once per frame
    void Update()
    {

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
}
