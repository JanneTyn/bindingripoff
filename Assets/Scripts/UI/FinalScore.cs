using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    int highestFloor;
    int enemiesKilled;
    float playerLevel;

    public void GetEndValues()
    {
        highestFloor = TestDifficultyScaler.instance.currentLevel + 1;
        enemiesKilled = StatDisplay.instance.enemiesKilled;
        playerLevel = GameObject.Find("TestPlayer").GetComponent<PlayerLeveling>().currentPlayerLevel;

        transform.Find("stat1").GetComponent<TMP_Text>().text = "Player Level: " + playerLevel;
        transform.Find("stat2").GetComponent<TMP_Text>().text = "Highest Floor Reached: " + highestFloor;
        transform.Find("stat3").GetComponent<TMP_Text>().text = "Enemies Killed: " + enemiesKilled;
    }
}
