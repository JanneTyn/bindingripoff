using System;
using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().inputActions.Gameplay.Pause.performed += Pause;
    }

    private void Update()
    {
        if (pauseMenu.activeSelf && Time.timeScale == 1f) { pauseMenu.SetActive(false); return; }
    }
    private void Pause(InputAction.CallbackContext context) => Pause();
    public void Pause()
    {
        if (GameObject.Find("UpgradingUI") == gameObject.activeSelf) { return; }
        pauseMenu.transform.SetAsLastSibling();
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

        DestroyAllObjectsByName("BossBar(Clone)");

        Time.timeScale = 0f;
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

    public void DestroyAllObjectsByName(string name)
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
            {
                Destroy(obj);
            }
        }
    }
}
