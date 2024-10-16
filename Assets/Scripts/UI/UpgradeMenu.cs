using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    //nyt kun alkaa olla enemm�n upgradeja niin huomaa ett� aika paskasti suunniteltu :D

    public string[] upgradesList = { "Health", "Health Regeneration", "Health Pickup", "Damage", "Fire Rate", "Defense", "Evasion", "IFrames", "Movement Speed", "Dash", "Luck" };
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
    public float healthRegenUpgrade = 1;
    public float healthRegenMaxCap = 10;
    public float dashCooldownUpgrade = -10;
    public float dashCooldownMaxCap = -80;
    public float luckUpgrade = 10;
    public float luckMaxCap = 100;

    public const string healingSuffix = "Healing";
    public const string damagingSuffix = "Damaging";
    public const string mobilitySuffix = "Mobility";
    public const string defenseSuffix = "Defense";
    public const string luckSuffix = "Luck";

    private List<float> allUpgrades = new List<float>();
    private int totalHPUpgrade = 0;
    private int totalFireRateUpgrade = 0;
    private int totalMovementSpeedUpgrade = 0;
    private int totalDamageUpgrade = 0;
    private int totalDefenseUpgrade = 0;
    private int totalDodgeUpgrade = 0;
    private float totaliframesUpgrade = 0;
    private float totalHealthPickUpUpgrade = 0;
    private float currenttotalHealthPickUpUpgrade = 0;
    private float totalHealthRegenUpgrade = 0;
    private float totalDashUpgrade = 0;
    private float totalLuckUpgrade = 0;
    public static float currentTotalLuckUpgrade = 0;
    private bool defenseCapped = false;
    private bool dodgeCapped = false;
    private bool iframesCapped = false;
    private bool healthPickUpCapped = false;
    private bool healthRegenCapped = false;
    private bool dashCapped = false;
    private bool luckCapped = false;

    private bool objectsInitialized = false;
    private Player player;  
    private GameObject upgradingUI;
    private GameObject upgradeOption1;
    private GameObject upgradeOption2;
    private GameObject upgradeOption3;
    private GameObject upgrademultiplierText;
    private GameObject upgradesuffixText1;
    private GameObject upgradesuffixText2;
    private GameObject upgradesuffixText3;
    private GameObject skipUpgradeButton;
    private GameObject upgradeStatsList;
    private TMP_Text upgradeStatsText;
    private List<TMP_Text> allStats = new List<TMP_Text>();
    private Canvas gameplayCanvas;

    private IEnumerable<GameObject> enemyhpBars;
    //private List<GameObject> enemyhpbars = new List<GameObject>();
    private List<GameObject> upgradeOptionList = new List<GameObject>();
    private List<GameObject> upgradeSuffixList = new List<GameObject>();
    private List<int> rolledUpgrades = new List<int>();
    private int rerollAttempts = 0;

    
    private void InitializeObjects()
    {
        player = GameObject.Find("TestPlayer").GetComponent<Player>();
        upgradeOption1 = GameObject.Find("Option1");
        upgradeOption2 = GameObject.Find("Option2");
        upgradeOption3 = GameObject.Find("Option3");
        upgradesuffixText1 = GameObject.Find("Option1/suffix");
        upgradesuffixText2 = GameObject.Find("Option2/suffix");
        upgradesuffixText3 = GameObject.Find("Option3/suffix");
        upgrademultiplierText = GameObject.Find("UpgradeMultiplierText");
        skipUpgradeButton = GameObject.Find("SkipUpgradeButton");
        upgradeStatsList = GameObject.Find("UpgradeList");
        upgradeStatsText = GameObject.Find("UpgradeList/UpgradeListText").GetComponent<TMP_Text>();
        upgradingUI = this.gameObject;
        upgradeOptionList.Add(upgradeOption1);
        upgradeOptionList.Add(upgradeOption2);
        upgradeOptionList.Add(upgradeOption3);
        upgradeSuffixList.Add(upgradesuffixText1);
        upgradeSuffixList.Add(upgradesuffixText2);
        upgradeSuffixList.Add(upgradesuffixText3);  
        InitializeUpgradesListText();
        InitializeCurrentStatsList();
        objectsInitialized = true;
        HealthPickUp.healMultiplier = 1.0f;
        EnemyLootDrop.healthChanceMultiplier = 1.0f;
        

    }
    public void OpenUpgradeMenu()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.audioClipListAsset.uiUpgrademenuOpen, transform.position);
        gameplayCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        EnableUI();
        if (!objectsInitialized) InitializeObjects();      
        Time.timeScale = 0f;
        player.GetComponent<Animator>().enabled = false;
        SetCurrentStats();
        RollUpgradeChoices();
    }

    void RollUpgradeChoices()
    {
        for (int i = 0; i < 3; i++)
        {
            while (true)
            {
                rerollAttempts++;
                int upgradeID = UnityEngine.Random.Range(0, upgradesList.Length);

                if (!rolledUpgrades.Contains(upgradeID))
                {
                    if (upgradesList[upgradeID] == "Defense" && defenseCapped) continue;
                    if (upgradesList[upgradeID] == "Evasion" && dodgeCapped) continue;
                    if (upgradesList[upgradeID] == "IFrames" && iframesCapped) continue;
                    if (upgradesList[upgradeID] == "Health Pickup" && healthPickUpCapped) continue;
                    if (upgradesList[upgradeID] == "Health Regeneration" && healthRegenCapped) continue;
                    if (upgradesList[upgradeID] == "Dash" && dashCapped) continue;
                    if (upgradesList[upgradeID] == "Luck" && luckCapped) continue;

                    rolledUpgrades.Add(upgradeID);
                    upgradeOptionList[i].GetComponent<TMP_Text>().text = upgradesList[upgradeID];
                    SetUpgradeDescription(upgradesList[upgradeID], i);
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
        skipUpgradeButton.GetComponent<Button>().enabled = false;

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
        skipUpgradeButton.GetComponent<Button>().enabled = true;
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
        AudioManager.instance.PlaySFX(AudioManager.instance.audioClipListAsset.uiUpgradeChosen, transform.position);
        Debug.Log("Selected " + ID);

        string upgradeName = upgradesList[ID];

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
                EnemyLootDrop.healthChanceMultiplier += totalHealthPickUpUpgrade / 100;
                HealthPickUp.healMultiplier += totalHealthPickUpUpgrade / 100;
                if (currenttotalHealthPickUpUpgrade >= healthPickUpMaxCap)
                {
                    healthPickUpCapped = true;
                }
                break;
            case "Health Regeneration":
                player.healthRegen += totalHealthRegenUpgrade;
                player.healthRegenEnabled = true;
                if (player.healthRegen >= healthRegenMaxCap)
                {
                    healthRegenCapped = true;
                }
                break;
            case "Dash":
                player.dashCooldownDecrease += totalDashUpgrade;
                if (player.dashCooldownDecrease <= dashCooldownMaxCap)
                {
                    dashCapped = true;
                }
                break;
            case "Luck":
                currentTotalLuckUpgrade += totalLuckUpgrade;
                if (currentTotalLuckUpgrade >= luckMaxCap)
                {
                    luckCapped = true;
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
                SetSuffixTitle(healingSuffix, i);
                break;
            case "Fire Rate":
                totalFireRateUpgrade = fireRateUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalFireRateUpgrade + "% " + upgName;
                SetSuffixTitle(damagingSuffix, i);
                break;
            case "Movement Speed":
                totalMovementSpeedUpgrade = movementSpeedUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalMovementSpeedUpgrade + "% " + upgName;
                SetSuffixTitle(mobilitySuffix, i);
                break;
            case "Damage":
                totalDamageUpgrade = damageUpgrade * UpgradeMultiplier;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalDamageUpgrade + "% " + upgName;
                SetSuffixTitle(damagingSuffix, i);
                break;
            case "Defense":
                totalDefenseUpgrade = defenseUpgrade * UpgradeMultiplier;
                if (player.armor - totalDefenseUpgrade < defenseMaxCap)
                {
                    totalDefenseUpgrade = (int)(defenseMaxCap - player.armor);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = totalDefenseUpgrade + "% damage taken";
                SetSuffixTitle(defenseSuffix, i);
                break;
            case "Evasion":
                totalDodgeUpgrade = dodgeUpgrade * UpgradeMultiplier;
                if (player.dodge + totalDodgeUpgrade > dodgeMaxCap)
                {
                    totalDodgeUpgrade = (int)(dodgeMaxCap - player.dodge);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalDodgeUpgrade + "% dodge chance";
                SetSuffixTitle(mobilitySuffix, i);
                break;
            case "IFrames":
                totaliframesUpgrade = iframesUpgrade * UpgradeMultiplier;
                if (player.iFramesLengthIncrease + totaliframesUpgrade > iframesMaxCap)
                {
                    totaliframesUpgrade = (iframesMaxCap - player.iFramesLengthIncrease);
                }
                float totalpercentage = totaliframesUpgrade / 100;
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalpercentage.ToString().Replace(",", ".") + "s iframes duration";
                SetSuffixTitle(defenseSuffix, i);
                break;
            case "Health Pickup":
                totalHealthPickUpUpgrade = healthPickUpUpgrade * UpgradeMultiplier;
                if (currenttotalHealthPickUpUpgrade + totaliframesUpgrade > healthPickUpMaxCap)
                {
                    totalHealthPickUpUpgrade = (healthPickUpMaxCap - currenttotalHealthPickUpUpgrade);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "Health Pickups +" + totalHealthPickUpUpgrade + "% Healing And Drop Rate";
                SetSuffixTitle(healingSuffix, i);
                break;
            case "Health Regeneration":
                totalHealthRegenUpgrade = healthRegenUpgrade * UpgradeMultiplier;
                if (player.healthRegen + totalHealthRegenUpgrade > healthRegenMaxCap)
                {
                    totalHealthRegenUpgrade = (healthRegenMaxCap - player.healthRegen);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "+" + totalHealthRegenUpgrade + "%/s Health Regeneration";
                SetSuffixTitle(healingSuffix, i);
                break;
            case "Dash":
                totalDashUpgrade = dashCooldownUpgrade * UpgradeMultiplier;
                if (player.dashCooldownDecrease - totalDashUpgrade < dashCooldownMaxCap)
                {
                    totalDashUpgrade = (int)(dashCooldownMaxCap - player.dashCooldownDecrease);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = totalDashUpgrade + "% Dash Cooldown";
                SetSuffixTitle(mobilitySuffix, i);
                break;
            case "Luck":
                totalLuckUpgrade = luckUpgrade * UpgradeMultiplier;
                if (currentTotalLuckUpgrade + luckUpgrade > luckMaxCap)
                {
                    totalLuckUpgrade = (int)(luckMaxCap - currentTotalLuckUpgrade);
                }
                upgradeOptionList[i].GetComponent<TMP_Text>().text = totalLuckUpgrade + "% More Weapon Drops";
                SetSuffixTitle(luckSuffix, i);
                break;
            default:
                upgradeOptionList[i].GetComponent<TMP_Text>().text = "Unknown Upgrade";
                break;
        }
    }

    public void SetSuffixTitle(string suffix, int i)
    {
        upgradeSuffixList[i].GetComponent<TMP_Text>().text = suffix;
        switch (suffix)
        {
            case healingSuffix:
                upgradeSuffixList[i].GetComponent<TMP_Text>().color = new Color(0.1f, 0.8f, 0.1f, 1);
                break;
            case damagingSuffix:
                upgradeSuffixList[i].GetComponent<TMP_Text>().color = new Color(0.8f, 0.1f, 0.1f, 1);
                break;
            case mobilitySuffix:
                upgradeSuffixList[i].GetComponent<TMP_Text>().color = new Color(0.1f, 0.1f, 0.8f, 1);
                break;
            case defenseSuffix:
                upgradeSuffixList[i].GetComponent<TMP_Text>().color = new Color(0.8f, 0.8f, 0.8f, 1);
                break;
            case luckSuffix:
                upgradeSuffixList[i].GetComponent<TMP_Text>().color = new Color(0.8f, 0.1f, 0.8f, 1);
                break;
        }
    }

    public void InitializeUpgradesListText()
    {
        allUpgrades.Clear();
        allUpgrades.Add(100 + player.maxHealthIncrease);
        allUpgrades.Add(player.healthRegen);
        allUpgrades.Add(currenttotalHealthPickUpUpgrade);
        allUpgrades.Add(player.damageIncrease);
        allUpgrades.Add(player.fireRate);
        allUpgrades.Add(Math.Abs(player.armor));
        allUpgrades.Add(player.dodge);
        allUpgrades.Add(player.iFramesLengthIncrease);
        allUpgrades.Add(player.moveSpeedIncrease);
        allUpgrades.Add(Math.Abs(player.dashCooldownDecrease));
        allUpgrades.Add(currentTotalLuckUpgrade);
    }
    public void InitializeCurrentStatsList()
    {
        for (int i = 0; i < upgradesList.Length; i++)
        {
            TMP_Text currentUpgradeText;
            currentUpgradeText = Instantiate(upgradeStatsText, transform.position, Quaternion.identity);
            allStats.Add(currentUpgradeText);
            currentUpgradeText.transform.SetParent(upgradeStatsList.transform, false);
            currentUpgradeText.text = upgradesList[i].ToString();
        }
    }

    public void SetCurrentStats()
    {      
        for (int i = 0; i < upgradesList.Length; i++)
        {
            if (i == 0) allStats[i].text = upgradesList[i].ToString() + " " + allUpgrades[i].ToString();
            else allStats[i].text = upgradesList[i].ToString() + " +" + allUpgrades[i].ToString() + "%";

            if ((allUpgrades[i] > 0 && i != 0) || (i == 0 && allUpgrades[i] > 100))
            {
                allStats[i].color = new Color(0.2f, 0.8f, 0.2f);
            }
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
        UpgradeMultiplier += 1;
        RemoveButtonListeners();
        DisableUI();
        ReturnToGame();
    }

    public void ReturnToGame()
    {
        InitializeUpgradesListText();
        player.GetComponent<Animator>().enabled = true;
        Time.timeScale = 1.0f;
    }
 
    void EnableUI()
    {
        this.gameObject.SetActive(true);
        gameplayCanvas.enabled = false;
        enemyhpBars = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "enemyhph");
    }

    void DisableUI()
    {
        this.gameObject.SetActive(false);     
        gameplayCanvas.enabled = true;
    }
}
