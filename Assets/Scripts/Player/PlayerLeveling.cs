using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    public float currentExperience = 0;
    public float currentPlayerLevel = 1;
    public float levelXPThreshold = 100;
    public float totalXPRequired = 100;
    public int skillPoints = 0;
    private float previousXp = 0; //for slider
    
    private PlayerStats stats;
    public UpgradeMenu upgradeMenu;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        //upgradeMenu = GameObject.Find("Canvas/UpgradingUI").GetComponent<UpgradeMenu>();
    }

    // Update is called once per frame

    public void IncreaseXP(float experience = 50)
    {
        currentExperience += experience * (TestDifficultyScaler.instance.currentRoomDifficulty);

        if (currentExperience >= totalXPRequired)
        {
            PlayerLevelUp();
        }
        StatDisplay.instance.UpdateXp(currentExperience - previousXp, levelXPThreshold);
    }

    private void PlayerLevelUp()
    {
        currentPlayerLevel++;
        previousXp += levelXPThreshold;
        upgradeMenu.OpenUpgradeMenu();
        SetNewLevelThreshold();
    }

    private void SetNewLevelThreshold()
    {
        levelXPThreshold = 30 + Mathf.Round(levelXPThreshold * 1.1f);
        Debug.Log("new threshold: " + levelXPThreshold);
        totalXPRequired = totalXPRequired + levelXPThreshold;
    }

}
