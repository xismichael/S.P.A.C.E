using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Planet planet;
    public GameObject planetinfoview;
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // Initialize any necessary components or settings
        UnityEngine.Debug.Log("Planet UI Initialized");
        //planet = PlanetDatabase
    }

    public void OnClick()
    {
        // planet Desscription Pop Up
        UnityEngine.Debug.Log("Planet UI clicked");

        if (planetinfoview != null)
        {
            planetinfoview.SetActive(true); 
        }
        else
        {
            UnityEngine.Debug.LogError("planetinfoview is not assigned!");
            return;
        }
        
    }
}
