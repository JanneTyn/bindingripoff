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
    
    public PlayerStats stats;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IncreaseXP(20);
        }

        if (skillPoints > 0)
        {
            AssignSkillPoints();
        }
    }

    public void IncreaseXP(float experience)
    {
        currentExperience += experience;

        if (currentExperience > totalXPRequired)
        {
            PlayerLevelUp();
        }
    }

    private void PlayerLevelUp()
    {
        currentPlayerLevel++;
        skillPoints++;
        SetNewLevelThreshold();
    }

    private void SetNewLevelThreshold()
    {
        levelXPThreshold = 30 + Mathf.Round(levelXPThreshold * 1.1f);
        Debug.Log("new threshold: " + levelXPThreshold);
        totalXPRequired = totalXPRequired + levelXPThreshold;
    }

    void AssignSkillPoints() //temporary num inputs, to be replaced with UI
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stats.IncreaseMaxHealth();
            skillPoints--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (stats.regenAmountCapped == false)
            {
                stats.IncreaseRegenAmount();
                skillPoints--;
            }
        }

    }

}
