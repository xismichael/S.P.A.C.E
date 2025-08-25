using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatureUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public Creature creature;
    public CreatureInfoView creatureInfoView;
    public Image creatureImage;
    public CreatureManager creatureManager;
    public Animator CreatureAnimator;

    void Start()
    {
        CreatureAnimator.Play(creature.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // Initialize any necessary components or settings
        

    }

    public void OnClick()
    {
        // Handle click events on the creature UI
        //creatureInfoView.Open(creature);
        creatureManager.SetCreatureUI(this);
        

        gameObject.SetActive(false);

    }
}
