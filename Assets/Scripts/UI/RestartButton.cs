using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// for testing ui
/// </summary>
public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("NewLevel", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
