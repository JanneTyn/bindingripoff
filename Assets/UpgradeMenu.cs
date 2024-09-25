using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private PlayerStats playerStats;
    public int UpgradeMultiplier = 1;
    private GameObject upgradingUI;

    private GameObject upgradeOption1;
    private GameObject upgradeOption2;
    private GameObject upgradeOption3;
    private GameObject upgrademultiplierText;

    private List<GameObject> upgradeOptionList = new List<GameObject>();
    private List<int> rolledUpgrades = new List<int>();
    private int rerollAttempts = 0;
    private void InitializeObjects()
    {
        playerStats = GameObject.Find("TestPlayer").GetComponent<PlayerStats>();
        upgradeOption1 = GameObject.Find("Option1");
        upgradeOption2 = GameObject.Find("Option2");
        upgradeOption3 = GameObject.Find("Option3");
        upgrademultiplierText = GameObject.Find("UpgradeMultiplierText");
        upgradingUI = this.gameObject;
        upgradeOptionList.Add(upgradeOption1);
        upgradeOptionList.Add(upgradeOption2);
        upgradeOptionList.Add(upgradeOption3);

    }
    public void OpenUpgradeMenu()
    {
        EnableUI();
        InitializeObjects();      
        Time.timeScale = 0f;      
        RollUpgradeChoices();
    }

    void RollUpgradeChoices()
    {
        for (int i = 0; i < 3; i++)
        {
            while (true)
            {
                rerollAttempts++;
                int upgradeID = UnityEngine.Random.Range(0, playerStats.upgrades.Count);

                if (!rolledUpgrades.Contains(upgradeID))
                {
                    rolledUpgrades.Add(upgradeID);
                    upgradeOptionList[i].GetComponent<TMP_Text>().text = playerStats.upgrades[upgradeID];
                    upgradeOptionList[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { UpgradeSelected(upgradeID); });
                    break;
                }
                else if (rerollAttempts > 1000)
                {
                    break;
                }
            }
        }
        rolledUpgrades.Clear();
        rerollAttempts = 0;

        UpgradeMultiplierCheck();
        
    }
    public void UpgradeMultiplierCheck()
    {
        if (UpgradeMultiplier > 1)
        {
            upgrademultiplierText.GetComponent<TMP_Text>().text = UpgradeMultiplier + "x Multiplier Active!";
        }
        else
        {
            upgrademultiplierText.GetComponent<TMP_Text>().text = "";
        }
    }
    public void UpgradeSelected(int ID)
    {
        Debug.Log("Selected " + ID);
        UpgradeMultiplier = 1;
        DisableUI();
        ReturnToGame();
    }

    public void SkipUpgrade()
    {
        UpgradeMultiplier++;
        DisableUI();
        ReturnToGame();
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1.0f;
    }
 
    void EnableUI()
    {
        this.gameObject.SetActive(true);
    }

    void DisableUI()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
