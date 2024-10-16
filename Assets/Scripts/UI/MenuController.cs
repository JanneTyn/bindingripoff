using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Singleton
    public static MenuController instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null && instance != this)
        {
            Debug.Log("Instance of MenuController already exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject gameplayUI;
    public GameObject deathUI;
    [SerializeField] private TMP_Text roomDepthText;
    [SerializeField] private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.inputActions.Gameplay.Pause.performed += Pause;
    }

    private void Pause(InputAction.CallbackContext context) => Pause();
    public void Pause()
    {
        if (deathUI.activeSelf || upgradeMenu.activeSelf) return;

        gameplayUI.SetActive(!gameplayUI.activeSelf);
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if(pauseMenu.activeSelf) //paused
        {
            StatDisplay.instance.DisplayCurrentStats();
            pauseMenu.transform.SetAsLastSibling();
            player.inputActions.Disable();
        }
        else //unpaused
        {
            player.inputActions.Enable();
        }
    }

    public void ExitToMenu()
    {
        var go = new GameObject("Tuhotaan kaikki!!! muahaha");
        DontDestroyOnLoad(go);

        foreach (var obj in go.scene.GetRootGameObjects())
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        var go = new GameObject("Tuhotaan kaikki!!! muahaha");
        DontDestroyOnLoad(go);

        foreach (var obj in go.scene.GetRootGameObjects())
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        }

        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void RoomDepth(int roomDepth)
    {
        roomDepthText.text = "Room depth: " + roomDepth;
    }

    internal void Death()
    {
        var bossBar = GameObject.Find("BossBar(Clone)");
        if(bossBar) bossBar.SetActive(false);

        player.inputActions.Disable();
        deathUI.SetActive(true);
        deathUI.transform.SetAsLastSibling();
        transform.Find("DeathScreen/Score").GetComponent<FinalScore>().GetEndValues();
        Time.timeScale = 0f;
        gameplayUI.SetActive(false);
    }
}
