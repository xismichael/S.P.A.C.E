using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public SpaceWindow spaceWindow;

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

}
