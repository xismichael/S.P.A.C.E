using UnityEngine;

public class PlanetUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Planet planet;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // Initialize any necessary components or settings
        Debug.Log("Planet UI Initialized");
        //planet = PlanetDatabase
    }

    public void OnClick()
    {
        // planet Desscription Pop Up
        Debug.Log("Planet UI clicked");
    }
}
