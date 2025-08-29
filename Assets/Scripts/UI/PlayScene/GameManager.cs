using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager Instance { get; private set; }

    //game timer
    [SerializeField] private CountdownTimer timer;
    [SerializeField] private PlanetManager planetManager;
    [SerializeField] private CreatureManager creatureManager;

    [SerializeField] private TextMeshProUGUI TimerText;

    [SerializeField] private ReportCard reportCard;

    [SerializeField] private TextMeshProUGUI ThoughtBubbleText;

    private Dictionary<string, float> StartingSanity = new Dictionary<string, float>();

    private int matchesMade = 0;
    private float totalScore = 0;

    private bool tenSecondsPassed = false;


    void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        timer.onTick.AddListener(OnTimerTick);
        timer.onCompleted.AddListener(OnTimerCompleted);
    }

    void OnDisable()
    {
        timer.onTick.RemoveListener(OnTimerTick);
        timer.onCompleted.RemoveListener(OnTimerCompleted);
    }

    void Start()
    {
        //testing
        // Planet testPlanet = PlanetDatabase.Instance.GetPlanet("GJ 1214 b");
        // Creature testCreature = CreatureDatabase.Instance.GetCreature("Walking Fish");

        // float rating = RatingSystem.GetCreaturePlanetRating(testCreature, testPlanet);
        // Debug.Log($"Rating for {testCreature.name} on {testPlanet.name}: {rating}");

        //FinalScoreText.text = "";
        StartGame();




    }

    // Update is called once per frame
    void Update()
    {

    }

    //Timer functions

    //function that is called every tick
    private void OnTimerTick(float remaining)
    {
        // Update your UI here
        //Debug.Log($"Time remaining: {remaining:F1} seconds");
        int total = Mathf.Max(0, Mathf.FloorToInt(remaining));
        TimerText.text = $"{total / 60}:{total % 60:00}";

        //run every 10 seconds
        if (!tenSecondsPassed && total % 30 == 0)
        {
            creatureManager.creatures = RatingSystem.updateSanity(creatureManager.creatures);
            creatureManager.UpdateCreatureUIs();
            UpdateThoughtBubble();
            tenSecondsPassed = true;
        }

        if (total % 30 != 0)
        {
            tenSecondsPassed = false;
        }

    }

    //function that is called when the timer completes
    private void OnTimerCompleted()
    {
        // Handle timer completion
        Debug.Log("Timer completed!");
        EndGame(GameEndReason.TimeUp);
    }
    public void StartGame()
    {
        // Reset game and set time
        matchesMade = 0;
        totalScore = 0;
        ThoughtBubbleText.text = "";
        timer.StartTimer(600f);
        planetManager.startGame();
        creatureManager.StartGame();
        reportCard.ClearMessages();
        SoundManager.Instance.PlayBackground(2);
        UpdateThoughtBubble();

    }
    private void EndGame(GameEndReason reason)
    {

        if (reason == GameEndReason.TimeUp)
        {
            foreach (Creature creature in creatureManager.creatures)
            {
                matchesMade++;
                reportCard.MatchMade($"{creature.name}: {0}");
            }
        }



        reportCard.ShowScore(totalScore / Mathf.Max(1, matchesMade));
        reportCard.Show();
        //Update UI results
    }

    public void OnMatchMade()
    {
        SoundManager.Instance.PlayClick(5); // ensure you actually have a 5th click

        var creature = creatureManager.SelectedCreature;
        var planet = planetManager.SelectedPlanet;

        // 1) Ask RatingSystem for the PENALTY BREAKDOWN (doesn't change your rating)
        var bd = RatingSystem.GetCreaturePlanetBreakdown(creature, planet);

        // 2) Keep your EXISTING rating math (raw × sanityFactor)
        float raw = RatingSystem.GetCreaturePlanetRating(creature, planet);
        float startingSanity = StartingSanity.TryGetValue(creature.name, out var startVal)
            ? startVal
            : Mathf.Max(0.0001f, creature.traits.sanity);
        float currentSanity = creature.traits.sanity;
        float sanityFactor = currentSanity / Mathf.Max(0.0001f, startingSanity);
        float finalScore = Mathf.Clamp(raw * sanityFactor, 0f, 100f);

        // 3) Build the flavorful line using the normalized culprit picker
        string line = RatingSystem.BuildReportMessage(
            creature.name, bd, sanityFactor, finalScore);

        // 4) Update totals/UI (now with message instead of raw score)
        totalScore += finalScore;
        matchesMade++;
        reportCard.MatchMade($"{line}  (grade {finalScore:F1})");

        // 5) Clean up selections
        planetManager.DeleteSelectedPlanet();
        creatureManager.DeleteSelectedCreature();

        if (AllMatchesComplete())
        {
            timer.StopTimer();
            EndGame(GameEndReason.AllMatched);
        }
    }


    private bool AllMatchesComplete()
    {
        // Your match-completion logic here
        if (planetManager.CurrentPlanets.Count == 0 && creatureManager.CurrentCreatures.Count == 0) return true;
        return false;
    }

    public float getTotalScore()
    {
        //return the end score of the game
        return 0f;
    }

    public float getRating(Creature creature, Planet planet)
    {
        // Use the RatingSystem and creature behavior on ship to get a rating based on creature and planet
        return 0f;
    }

    public void SendCreatureToPlanet()
    {
        if (planetManager.selectedPlanetUI == null || creatureManager.selectedCreatureUI == null) return;
        OnMatchMade();


    }

    public void UpdateThoughtBubble()
    {
        string sb = "";

        foreach (Creature creature in creatureManager.creatures)
        {

            float startingSanity = StartingSanity[creature.name];
            float currentSanity = creature.traits.sanity;

            float ratio = currentSanity / startingSanity;
            Debug.Log($"Creature: {creature.name}, Ratio: {ratio}");

            string status;
            if (ratio >= 0.95f) status = "Ecstatic";
            else if (ratio >= 0.90f) status = "Happy";
            else if (ratio >= 0.85f) status = "Content";
            else if (ratio >= 0.80f) status = "Uneasy";
            else if (ratio >= 0.75f) status = "Irritated";
            else if (ratio >= 0.70f) status = "Unhappy";
            else if (ratio >= 0.65f) status = "Angry";
            else status = "Mad";

            sb += $"{creature.name}: {status}\n";
        }

        ThoughtBubbleText.text = sb;
    }


    public void BuildSanityDictionary()
    {
        Dictionary<string, float> sanityDict = new Dictionary<string, float>();

        foreach (Creature creature in creatureManager.creatures)
        {
            sanityDict[creature.name] = creature.traits.sanity;
            Debug.Log($"Starting sanity for {creature.name}: {creature.traits.sanity}");
        }
        StartingSanity = sanityDict;
    }

    public void OnMenuButton()
    {
        //load the main menu scene
        SceneManager.LoadScene("MenuScreen");
    }

    public void OnRestartButton()
    {
        //restart the play scene
        StartGame();
    }

}
