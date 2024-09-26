using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private bool isPaused;
    public GameObject pauseMenu;
    public GameObject upgradeMenu;

    void Start()
    {
        Pause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!upgradeMenu.activeSelf)
            {
                if (!isPaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            
            }
        }
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game     
        pauseMenu.SetActive(true); // Show the pause menu UI

    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unpause the game
        pauseMenu.SetActive(false); // Hide the pause menu UI
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("UItest2");
    }
}
