using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomDepthText : MonoBehaviour
{
    #region Singleton
    public static RoomDepthText instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Instance of RoomDepthText already exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    private TMP_Text roomText;

    private void Start()
    {
        roomText = GetComponent<TMP_Text>();
    }
    public void RoomDepth(int roomDepth)
    {
        roomText.text = "Rooms Depth Level: " + roomDepth;
    }
}
