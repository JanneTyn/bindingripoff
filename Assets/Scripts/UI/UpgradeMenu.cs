using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public int UpgradeMultiplier = 1;
    public int baseHPUpgrade = 10; //every upgrade has the base increase
    public int extraHPUpgrade = 0; //rises by x amount every upgrade
    public int fireRateUpgrade = 15;
    public int movementSpeedUpgrade = 20;
    public int damageUpgrade = 20;
    public int defenseUpgrade = 5;
    public int defenseMaxCap = 60;
    public int dodgeUpgrade = 5;
    public int dodgeMaxCap = 60;
    public float iframesUpgrade = 5;
    public float iframesMaxCap = 50;
    public float healthPickUpUpgrade = 20;
    public float healthPickUpMaxCap = 100;


    private int totalHPUpgrade = 0;
    private int totalFireRateUpgrade = 0;
    private int totalMovementSpeedUpgrade = 0;
    private int totalDamageUpgrade = 0;
    private int totalDefenseUpgrade = 0;
    private int totalDodgeUpgrade = 0;
    private float totaliframesUpgrade = 0;
    private float totalHealthPickUpUpgrade = 0;
    private float currenttotalHealthPickUpUpgrade = 0;
    private bool defenseCapped = false;
    private bool dodgeCapped = false;
    private bool iframesCapped = false;
    private bool healthPickUpCapped = false;

    private bool objectsInitialized = false;
    private PlayerStats playerStats;  
    private Player player;  
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
        player = GameObject.Find("TestPlayer").GetComponent<Player>();
        upgradeOption1 = GameObject.Find("Option1");
        upgradeOption2 = GameObject.Find("Option2");
        upgradeOption3 = GameObject.Find("Option3");
        upgrademultiplierText = GameObject.Find("UpgradeMultiplierText");
        upgradingUI = this.gameObject;
        upgradeOptionList.Add(upgradeOption1);
        upgradeOptionList.Add(upgradeOption2);
        upgradeOptionList.Add(upgradeOption3);
        objectsInitialized = true;
        HealthPickUp.healMultiplier = 1.0f;
        EnemyLootDrop.chanceMultiplier = 1.0f;

    }
    public void OpenUpgradeMenu()
    {
        EnableUI();
        if (!objectsInitialized) InitializeObjects();      
        Time.timeScale = 0f;
        player.GetComponent<Animator>().enabled = false;
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
                    if (playerStats.upgrades[upgradeID] == "Defense" && defenseCapped) continue;
                    if (playerStats.upgrades[upgradeID] == "Evasion" && dodgeCapped) continue;
                    if (playerStats.upgrades[upgradeID] == "IFrames" && iframesCapped) continue;
                    if (playerStats.upgrades[upgradeID] == "Health Pickup" && healthPickUpCapped) continue;

                    rolledUpgrades.Add(upgradeID);
                    upgradeOptionList[i].GetComponent<TMP_Text>().text = playerStats.upgrades[upgradeID];
                    SetUpgradeDescription(playerStats.upgrades[upgradeID], i);
                    upgradeOptionList[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { UpgradeSelected(upgradeID); });
                    upgradeOptionList[i].transform.GetChild(0).GetComponent<Button>().enabled = false;
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
        StartCoroutine(OptionBufferDelay());
        
    }

    IEnumerator OptionBufferDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 3; i++)
        {
            upgradeOptionList[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
        }
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

        string upgradeName = playerStats.upgrades[ID];

        switch (upgradeName) {
            case "Health":
                player.maxHealthIncrease += totalHPUpgrade;
                extraHPUpgrade += 5;
                break;
            case "Fire Rate":
                player.fireRate += totalFireRateUpgrade;
                break;
            case "Movement Speed":
                player.moveSpeedIncrease += totalMovementSpeedUpgrade;
                break;
            case "Damage":
                player.damageIncrease += totalDamageUpgrade;
                break;
            case "Defense":
                player.armor += totalDefenseUpgrade;
                if (player.armor <= defenseMaxCap)
                {
                    defenseCapped = true;
                }
                break;
            case "Evasion":
                player.dodge += totalDodgeUpgrade;
                if (player.dodge >= dodgeMaxCap)
                {
                    dodgeCapped = true;
                }
                break;
            case "IFrames":
                player.iFramesLengthIncrease += totaliframesUpgrade;
                if (player.iFramesLengthIncrease >= iframesMaxCap)
                {
                    iframesCapped = true;
                }
                break;
            case "Health Pickup":
                currenttotalHealthPickUpUpgrade += totalHealthPickUpUpgrade;
                EnemyLootDrop.chanceMultiplier += totalHealthPickUpUpgrade / 100;
                HealthPickUp.healMultiplier += totalHealthPickUpUpgrade / 100;
                if (currenttotalHealthPickUpUpgrade >= healthPickUpMaxCap)
                {
                    healthPickUpCapped = true;
                }
                break;
        }

        UpgradeMultiplier = 1;
        RemoveButtonListeners();
        DisableUI();
        ReturnToGame();
    }

    public void SetUpgradeDescription(string upgName, int i)
    {
        switch (upgName)
        {
            case "Health":
                totalHPUpgrade = (baseHPUpgrade + extraHPUpgrade) * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalHPUpgrade + " " + upgName;
                break;
            case "Fire Rate":
                totalFireRateUpgrade = fireRateUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalFireRateUpgrade + "% " + upgName;
                break;
            case "Movement Speed":
                totalMovementSpeedUpgrade = movementSpeedUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalMovementSpeedUpgrade + "% " + upgName;
                break;
            case "Damage":
                totalDamageUpgrade = damageUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalDamageUpgrade + "% " + upgName;
                break;
            case "Defense":
                totalDefenseUpgrade = defenseUpgrade * UpgradeMultiplier;
                if (player.armor - totalDefenseUpgrade < defenseMaxCap)
                {
                    totalDefenseUpgrade = (int)(defenseMaxCap - player.armor);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = totalDefenseUpgrade + "% damage taken";
                break;
            case "Evasion":
                totalDodgeUpgrade = dodgeUpgrade * UpgradeMultiplier;
                if (player.dodge + totalDodgeUpgrade > dodgeMaxCap)
                {
                    totalDodgeUpgrade = (int)(dodgeMaxCap - player.dodge);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalDodgeUpgrade + "% dodge chance";
                break;
            case "IFrames":
                totaliframesUpgrade = iframesUpgrade * UpgradeMultiplier;
                if (player.iFramesLengthIncrease + totaliframesUpgrade > iframesMaxCap)
                {
                    totaliframesUpgrade = (iframesMaxCap - player.iFramesLengthIncrease);
                }
                float totalpercentage = totaliframesUpgrade / 100;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalpercentage + "s iframes duration";
                break;
            case "Health Pickup":
                totalHealthPickUpUpgrade = healthPickUpUpgrade * UpgradeMultiplier;
                if (currenttotalHealthPickUpUpgrade + totaliframesUpgrade > healthPickUpMaxCap)
                {
                    totalHealthPickUpUpgrade = (healthPickUpMaxCap - currenttotalHealthPickUpUpgrade);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "Health Pickups +" + totalHealthPickUpUpgrade + "% Healing And Drop Rate";
                break;
            default:
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "Unknown Upgrade";
                break;
        }
    }

    public void RemoveButtonListeners()
    {
        for (int i = 0; i < 3; i++)
        {
            upgradeOptionList[i].transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void SkipUpgrade()
    {
        UpgradeMultiplier++;
        RemoveButtonListeners();
        DisableUI();
        ReturnToGame();
    }

    public void ReturnToGame()
    {
        player.GetComponent<Animator>().enabled = true;
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
