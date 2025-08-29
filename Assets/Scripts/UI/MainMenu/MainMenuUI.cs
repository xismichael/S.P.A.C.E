using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject creditsPanel;
    public GameObject backgroundStoryPanel;
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
        SoundManager.Instance.PlayClick(3);
        backgroundStoryPanel.SetActive(true);
    }

    public void OnBackgroundStoryExitButton()
    {
        SoundManager.Instance.PlayClick(4);
        backgroundStoryPanel.SetActive(false);
    }
}