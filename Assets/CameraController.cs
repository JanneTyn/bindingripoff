using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than 1 instance of CameraController exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    public void MoveCamera(Vector2 direction)
    {
        transform.Translate(direction);
    }
}
