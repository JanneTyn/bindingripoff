using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
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
        if (GameObject.Find("UpgradingUI") == gameObject.activeSelf) { return; }
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if(GameObject.Find("statstext") == gameObject.activeSelf)
        {
            StatDisplay.instance.DisplayCurrentStats();
        }
    }

    public void QuitToMenu()
    {
        GameObject player = GameObject.Find("TestPlayer");
        player.transform.position = Vector3.zero;
        player.GetComponent<Player>().currentHealth = 100f;

        CameraController.instance.currentCameraPos = new Vector3(0, -0.720000029f, -10);
        SceneManager.LoadScene("NewLevel", LoadSceneMode.Single);

        Time.timeScale = 0f;
    }
}
