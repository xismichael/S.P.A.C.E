using UnityEngine;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public Creature creature;
    public CreatureInfoView creatureInfoView;
    public Image creatureImage;
    public CreatureManager creatureManager;
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
        Debug.Log("Creature UI Initialized");
    }

    public void OnClick()
    {
        // Handle click events on the creature UI
        creatureInfoView.Open();
        creatureManager.SetCreatureUI(this);

    }
}
