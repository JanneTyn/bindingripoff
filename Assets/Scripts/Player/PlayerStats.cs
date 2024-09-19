using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float regenSpeed = 3.0f; //in seconds
    public float regenAmount = 0; //% of health
    public float defense = 0; //dmg resistance % increase
    public float evasion = 0;
    public float movementSpeed = 0; //only to apply buff/debuff
    public float luck = 0;

    public bool regenAmountCapped = false;

    public int baseHealthUpgrade = 10;
    private int totalBonusHealthUpgrade = 0; //increases on every max hp upgrade
    public int extraHPUpgrade = 5;

    public float regenAmountIncreasePerUpgrade = 0.005f;
    public float regenAmountCap = 0.1f;

    public void IncreaseMaxHealth()
    {
        maxHealth = maxHealth + (baseHealthUpgrade + totalBonusHealthUpgrade);
        totalBonusHealthUpgrade += extraHPUpgrade;
    }

    public void IncreaseRegenAmount()
    {
        regenAmount += regenAmountIncreasePerUpgrade;
        if (regenAmount >= regenAmountCap)
        {
            regenAmountCapped = true;
        }
    }
}
