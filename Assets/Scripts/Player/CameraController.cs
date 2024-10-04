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
        DontDestroyOnLoad(gameObject);
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

    public Vector3 currentCameraPos;
    Vector3 nextCameraPos;
    private bool cameraTransitioning;
    [SerializeField] private float cameraTransitionLength;
    private Vector3 refVelocity = Vector3.zero;

    public void StartShake(float duration, float amount)
    {
        shakeAmount = amount;
        shakeDuration = duration;
    }

    public void MoveCamera(Vector2 newPos)
    {
        cameraTransitioning = true;
        nextCameraPos = transform.position + (Vector3)newPos;
        currentCameraPos = transform.position;
    }

    private void Update()
    {
        
        if(cameraTransitioning)
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextCameraPos, ref refVelocity, cameraTransitionLength);
            if ((nextCameraPos - transform.position).magnitude < 0.05f)
            {
                cameraTransitioning = false;
                currentCameraPos = transform.position;
            }
        }
        else
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
}
