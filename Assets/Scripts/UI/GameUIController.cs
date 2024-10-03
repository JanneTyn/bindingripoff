using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUIController : MonoBehaviour
{
    #region Singleton
    public static GameUIController instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null && instance != this)
        {
            Debug.Log("Instance of GameUIController already exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    public GameObject deathUI;
    [SerializeField] private TMP_Text roomDepthText;

    public void RestartGame()
    {
        deathUI.SetActive(false);
      //  GameObject.Find("TestPlayer").transform.position = Vector3.zero;
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void RoomDepth(int roomDepth)
    {
        roomDepthText.text = "Rooms Depth Level: " + roomDepth;
    }
}
