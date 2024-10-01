using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool isPaused;
    private StatDisplay UI;
    private GameObject pauseMenu;
    private GameObject upgradeMenu;

    void Start()
    {
        gameObject.AddComponent<StatDisplay>();
        UI = GetComponent<StatDisplay>();
        UI.EnableGameobject("UpgradingUI");
        upgradeMenu = GameObject.Find("UpgradingUI");
        upgradeMenu.SetActive(false);

        Buttons();

        pauseMenu = GameObject.Find("pause(Clone)");
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

    private void Buttons()
    {
        Button continueButton = GameObject.Find("Continue").GetComponent<Button>();
        continueButton.onClick.AddListener(Resume);

        Button restartButton = GameObject.Find("Restart").GetComponent<Button>();
        restartButton.onClick.AddListener(QuitToMenu);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unpause the game
        pauseMenu.SetActive(false); // Hide the pause menu UI
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
