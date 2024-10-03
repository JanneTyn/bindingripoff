using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDifficultyScaler : MonoBehaviour
{
    #region Singleton
    public static TestDifficultyScaler instance;
    public static int currentLevel = 0;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Multiple instances of TestDifficultyScaler");
            enabled = false;
        }
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion


    private float time;
    [SerializeField] public float currentRoomDifficulty;
    [SerializeField] public float generalDifficultyMultiplier;
    [Range(1, 10)] public float levelDifficultyMultiplier = 2f;
    [Range(0, 10)] public float roomDepthDifficultyMultiplier = 0.3f;
    [Range(1, 10)] private float timeDifficultyMultiplier;
    [Range(60, 900)] public float timeDifficultyScaler = 300;
    [Range(1, 10)] public float enemyCountScaler;
    [Range(1, 10)] private float enemyCount;
    [Range(1, 10)] public float enemyHealthMultiplier;
    [Range(1, 10)] private float enemyHealthMultiplierFinal;
    [Range(1, 10)] public float enemyDamageMultiplier;
    [Range(1, 10)] private float enemyDamageMultiplierFinal;


    private void Start()
    {
        currentLevel = 0;
    }
    private void Update()
    {
        time += Time.deltaTime;
        timeDifficultyMultiplier = time / timeDifficultyScaler;

        generalDifficultyMultiplier = 1 + timeDifficultyMultiplier + (levelDifficultyMultiplier * currentLevel);
    }

    public float[] SetCurrentRoomDifficulty(int distanceToStartRoom)
    {
        currentRoomDifficulty = generalDifficultyMultiplier + (distanceToStartRoom * roomDepthDifficultyMultiplier);
        Debug.Log("Difficulty multiplier in this room: " + currentRoomDifficulty);
        enemyCount = Random.Range(3 + enemyCountScaler + currentLevel + Mathf.RoundToInt(distanceToStartRoom / 2), 7 + enemyCountScaler + currentLevel + Mathf.RoundToInt(distanceToStartRoom / 2));
        if (enemyCount > 20) enemyCount = 20;
        enemyHealthMultiplierFinal = currentRoomDifficulty * enemyHealthMultiplier;
        enemyDamageMultiplierFinal = currentRoomDifficulty * enemyDamageMultiplier;
        float[] modifiers = {enemyCount, enemyHealthMultiplierFinal, enemyDamageMultiplierFinal};
        return modifiers;
    }
}
