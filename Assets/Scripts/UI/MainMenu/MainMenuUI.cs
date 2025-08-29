using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject creditsPanel;
    public GameObject backgroundStoryPanel;
    public GameObject instructionsPanel;
    void Start()
    {
        // Initialize any necessary components or settings
        SoundManager.Instance.PlayBackground(1);
    }
    public void OnPlayButton()
    {
        //load the play scene
        SceneManager.LoadScene("PlayScreen");
        SoundManager.Instance.PlayClick(5);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    public void OnCreditButton()
    {
        //load credit scene
        SoundManager.Instance.PlayClick(1);
        creditsPanel.SetActive(true);
    }

    public void OnCreditExitButton()
    {
        //exit the game
        SoundManager.Instance.PlayClick(2);
        creditsPanel.SetActive(false);
    }

    public void OnBackgroundStoryButton()
    {
        SoundManager.Instance.PlayClick(1);
        backgroundStoryPanel.SetActive(true);
    }

    public void OnBackgroundStoryExitButton()
    {
        SoundManager.Instance.PlayClick(2);
        backgroundStoryPanel.SetActive(false);
    }

    public void OnInstructionsButton()
    {
        //load instruction scene
        SoundManager.Instance.PlayClick(1);
        instructionsPanel.SetActive(true);
    }

    public void OnInstructionsExitButton()
    {
        //exit the game
        SoundManager.Instance.PlayClick(2);
        instructionsPanel.SetActive(false);
    }
}