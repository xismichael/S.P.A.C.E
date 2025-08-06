using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        // Initialize any necessary components or settings
        Debug.Log("Main Menu Initialized");
    }
    public void OnPlayButton()
    {
        //load the play scene
        Debug.Log("Play button clicked");
        //SceneManager.LoadScene("PlayScene");
    }

    public void OnCreditButton()
    {
        //load credit scene
        Debug.Log("Credits button clicked");
    }
}