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

        GameObject player = GameObject.Find("TestPlayer");
        player.transform.position = Vector3.zero;

        Player playerComponent = player.GetComponent<Player>();
        playerComponent.currentHealth = 100f;
        playerComponent.animator.SetBool("paused", false);


        CameraController.instance.currentCameraPos = new Vector3(0, -0.720000029f, -10);
        SceneManager.LoadScene("NewLevel", LoadSceneMode.Single);

        Time.timeScale = 1f;
    }

    public void RoomDepth(int roomDepth)
    {
        roomDepthText.text = "Rooms Depth Level: " + roomDepth;
    }
}
