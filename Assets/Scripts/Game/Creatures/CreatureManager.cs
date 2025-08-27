using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CreatureManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private RectTransform RT;
    [SerializeField] private CreatureInfoView creatureInfoView;
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private Image SelectedCreatureImage;
    [SerializeField] private Sprite defaultCreatureSprite;
    public CreatureUI selectedCreatureUI = null;
    public Creature SelectedCreature = null;

    public int CreatureCount = 5;
    public List<CreatureUI> CurrentCreatures = new();
    public List<Creature> creatures;

    [SerializeField]
    private List<Vector2> creaturePositions = new()
    {
        new Vector2(-400, -300),
        new Vector2(-250, -350),
        new Vector2(-100, -300),
        new Vector2(100, -350),
        new Vector2(400, -300)
    };




    void Start()
    {

        creatures = CreatureDatabase.Instance.GetUniqueCreatures(CreatureCount);
        // How many can we actually place without repeating a slot?
        int spawnCount = Mathf.Min(creatures.Count, creaturePositions.Count);
        if (creatures.Count > creaturePositions.Count)
            Debug.LogWarning($"Requested {creatures.Count} creatures but only {creaturePositions.Count} positions. Spawning {spawnCount}.");

        // Clear any old UI (optional)
        foreach (var ui in CurrentCreatures) if (ui) Destroy(ui.gameObject);
        CurrentCreatures.Clear();

        // Sequentially assign positions
        for (int i = 0; i < spawnCount; i++)
        {
            var go = Instantiate(creaturePrefab, RT);
            var rt = go.GetComponent<RectTransform>();
            if (rt)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = creaturePositions[i]; // unique slot, no repeats
            }

            var ui = go.GetComponent<CreatureUI>();
            ui.creature = creatures[i];
            ui.creatureInfoView = creatureInfoView;
            ui.creatureManager = this; // Set the manager reference
            // ui.manager = this; // if your PlanetUI needs a back-reference
            ui.Initialize();

            CurrentCreatures.Add(ui);
        }

        GameManager.Instance.BuildSanityDictionary();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCreatureUI(CreatureUI creatureUI)
    {
        if (selectedCreatureUI != null)
        {
            selectedCreatureUI.gameObject.SetActive(true);
            selectedCreatureUI.CreatureAnimator.Play(selectedCreatureUI.creature.name);
        }
        selectedCreatureUI = creatureUI;
        SelectedCreature = creatureUI.creature;
        SelectedCreatureImage.sprite = creatureUI.creatureImage.sprite;
        creatureInfoView.UpdateInfoView(SelectedCreature);
    }

    public void DeleteSelectedCreature()
    {

        // remove from tracking lists
        CurrentCreatures.Remove(selectedCreatureUI);
        creatures.Remove(SelectedCreature);
        SelectedCreatureImage.sprite = null;

        // destroy the UI object
        Destroy(selectedCreatureUI.gameObject);

        // clear selection
        selectedCreatureUI = null;
        SelectedCreature = null;
        SelectedCreatureImage.sprite = defaultCreatureSprite;
    }

    public void UpdateCreatureUIs()
    {
        //print sanity points for each creature
        foreach (CreatureUI ui in CurrentCreatures)
        {
            Debug.Log($"{ui.creature.name} sanity: {ui.creature.traits.sanity}");
        }
    }

    public void ShowInfo()
    {
        if (selectedCreatureUI == null) return;
        creatureInfoView.Open(SelectedCreature);
    }
}
