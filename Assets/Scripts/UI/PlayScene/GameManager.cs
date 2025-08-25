using UnityEngine;
using TMPro;

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
        DontDestroyOnLoad(gameObject); // Optional: Keep this between scene loads
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
        matchesMade = 0;
        totalScore = 0;
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
        if (!tenSecondsPassed && total % 10 == 0)
        {
            creatureManager.creatures = RatingSystem.updateSanity(creatureManager.creatures);
            creatureManager.UpdateCreatureUIs();
            tenSecondsPassed = true;
        }

        if (total % 10 != 0)
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
        timer.StartTimer(300f);
        planetManager.startGame();
    }
    private void EndGame(GameEndReason reason)
    {
        Debug.Log($"Game ended because: {reason}");
        reportCard.ShowScore(totalScore / Mathf.Max(1, matchesMade));
        reportCard.Show();
        //Update UI results
    }

    public void OnMatchMade()
    {
        // If all matches are made, stop timer and end game

        float score = RatingSystem.GetCreaturePlanetRating(creatureManager.SelectedCreature, planetManager.SelectedPlanet);
        totalScore += score;
        matchesMade++;
        reportCard.MatchMade($"{creatureManager.SelectedCreature.name} has been sent to {planetManager.SelectedPlanet.name} with a score of {score}");
        planetManager.DeleteSelectedPlanet();
        creatureManager.DeleteSelectedCreature();
        if (AllMatchesComplete())
        {
            //FinalScoreText.text += $"\n your final scsore is: {totalScore/matchesMade}";
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

}
