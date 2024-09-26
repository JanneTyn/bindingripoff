using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    private PlayerLeveling Stat;
    public GameObject CurrentLevelText;
    void Start()
    {
        Stat = GameObject.Find("TestPlayer").GetComponent<PlayerLeveling>();
    }

    void Update()
    {
        CurrentLevelText.GetComponent<TMP_Text>().text = "Player level: " + Stat.currentPlayerLevel;
    }
}
