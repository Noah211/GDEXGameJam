using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Noah Logan

public class MainMenuButtons : MonoBehaviour
{

    public void PlayButton()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
