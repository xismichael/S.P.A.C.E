using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject creditsPanel;
    public GameObject backgroundStoryPanel;
    void Start()
    {
        // Initialize any necessary components or settings
    }
    public void OnPlayButton()
    {
        //load the play scene
        SceneManager.LoadScene("PlayScreen");
    }

    public void OnCreditButton()
    {
        //load credit scene
        creditsPanel.SetActive(true);
    }

    public void OnCreditExitButton()
    {
        //exit the game
        creditsPanel.SetActive(false);
    }

    public void OnBackgroundStoryButton()
    {
        backgroundStoryPanel.SetActive(true);
    }

    public void OnBackgroundStoryExitButton()
    {
        backgroundStoryPanel.SetActive(false);
    }
}