using UnityEngine;

public class SpaceWindow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isOpen;
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
        Debug.Log("Space Window Initialized");
        isOpen = false;
    }

    public void OpenWindow()
    {
        if (isOpen) return;
        isOpen = true;
        Debug.Log("window clicked");
    }

    public void CloseWindow()
    {
        if (!isOpen) return;
        isOpen = false;
        Debug.Log("window closed");
    }
}
