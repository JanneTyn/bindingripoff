using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement of camera
/// TODO smoothing, shakes, etc.
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController instance;
    
    private void Awake()
    {
        currentCameraPos = transform.position;
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than 1 instance of CameraController exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    private float shakeDuration = 0f;
    private float shakeAmount = 0f;
    private float decreaseFactor = 1f;

    Vector3 currentCameraPos;

    public void StartShake(float duration, float amount)
    {
        shakeAmount = amount;
        shakeDuration = duration;
    }

    public void MoveCamera(Vector2 direction)
    {
        transform.Translate(direction);
        currentCameraPos = transform.position;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = currentCameraPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.unscaledDeltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = currentCameraPos;
        }
    }
}
