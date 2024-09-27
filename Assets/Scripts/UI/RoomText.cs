using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

public class RoomText : MonoBehaviour
{
    #region Singleton
    public static RoomText instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Instance of RoomText already exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    private int roomsCleared = 0;
    private TMP_Text roomText;

    private void Start()
    {
        roomText = GetComponent<TMP_Text>();
    }
    public void RoomCleared()
    {
        roomsCleared++;
        roomText.text = "Rooms cleared: " + roomsCleared;
    }
}
