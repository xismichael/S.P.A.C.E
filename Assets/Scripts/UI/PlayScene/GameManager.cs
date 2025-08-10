using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager Instance { get; private set; }

    //game timer
    [SerializeField] private CountdownTimer timer;
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
        Planet testPlanet = PlanetDatabase.Instance.GetPlanet("test");
        Creature testCreature = CreatureDatabase.Instance.GetCreature("test");

        float rating = RatingSystem.GetCreaturePlanetRating(testCreature, testPlanet);

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
        Debug.Log($"Time remaining: {remaining:F1} seconds");
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
    }
    private void EndGame(GameEndReason reason)
    {
        Debug.Log($"Game ended because: {reason}");
        //Update UI results
    }

    public void OnMatchMade()
    {
        // If all matches are made, stop timer and end game
        if (AllMatchesComplete())
        {
            timer.StopTimer();
            EndGame(GameEndReason.AllMatched);
        }
    }

    private bool AllMatchesComplete()
    {
        // Your match-completion logic here
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

}
