using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {

        //play button open sound
        SoundManager.Instance.PlayClick(1);
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        //play button close sound
        SoundManager.Instance.PlayClick(2);

        gameObject.SetActive(false);
    }

    public void Replay()
    {
        //play button replay sound
        SoundManager.Instance.PlayClick(5);

        //reload the current scene
        Exit();
        GameManager.Instance.StartGame();
    }

    public void ExitToMainMenu()
    {
        //play button exit sound
        SoundManager.Instance.PlayClick(2);

        //load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScreen");
    }



}
