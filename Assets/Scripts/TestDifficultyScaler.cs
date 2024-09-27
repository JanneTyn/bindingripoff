using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDifficultyScaler : MonoBehaviour
{
    #region Singleton
    public static TestDifficultyScaler instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Multiple instances of TestDifficultyScaler");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    private float time;
    public float enemyHealthMultiplier;
    public float enemyCountMultiplier;

    private void Update()
    {
        time += Time.deltaTime;
    }
}
