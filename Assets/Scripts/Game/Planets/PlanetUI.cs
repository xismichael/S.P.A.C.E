using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Planet planet;
    public PlanetInfoView planetinfoview;
    public Image planetImage;

    public PlanetManager planetManager;
    public Button button;
    public Animator PlanetAnimator;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        PlanetAnimator.Play(planet.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // Initialize any necessary components or settings
        UnityEngine.Debug.Log(planet.name + " Initialized");
        //planet = PlanetDatabase
    }

    public void OnClick()
    {
        // planet Desscription Pop Up



        //play planet open sound
        SoundManager.Instance.PlayClick(4);

        //planetinfoview.Open(planet);
        planetManager.SetPlanetUI(this);
        
    }
}
