using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorruptTimer : MonoBehaviour
{
    public float timeBeforeCorruptionActivates = 120;
    public bool corruptionActive = false;
    public float corruptionTimeActive = 0;
    public float corruptionTimeToReachMax = 120; //the 
    public Color lerpedColor = Color.white;
    public static float corruptPercentage = 0;

    void Start()
    {
        StartCoroutine(CorruptionActivationTimer());
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += ResetCorruption;
    }

    private void ResetCorruption(Scene arg0, Scene arg1)
    {
        corruptionActive = false;
        corruptPercentage = 0;
        lerpedColor = Color.white;
        corruptionTimeActive = 0f;

        if (arg1.name != "BossScene") StartCoroutine(CorruptionActivationTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (corruptionActive)
        {
            if (corruptionTimeActive < corruptionTimeToReachMax)
            {
                corruptionTimeActive += Time.deltaTime;
                lerpedColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(corruptionTimeActive / corruptionTimeToReachMax, 1));
                corruptPercentage = 1 - lerpedColor.g;
            }
        }
    }

    private IEnumerator CorruptionActivationTimer()
    {
        yield return new WaitForSeconds(timeBeforeCorruptionActivates);
        corruptionActive = true;
    }
}
