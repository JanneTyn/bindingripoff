using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;

    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().inputActions.Gameplay.Pause.performed += Pause;
        Pause();
    }

    private void Pause(InputAction.CallbackContext context) => Pause();
    public void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if(GameObject.Find("statstext") == gameObject.activeSelf)
        {
            StatDisplay.instance.DisplayCurrentStats();
        }
    }

    public void QuitToMenu()
    {
        GameObject.Find("TestPlayer").transform.position = Vector3.zero;
        SceneManager.LoadScene("NewLevel", LoadSceneMode.Single);
    }
}
