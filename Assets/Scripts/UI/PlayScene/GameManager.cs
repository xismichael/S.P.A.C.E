using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager Instance { get; private set; }
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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
