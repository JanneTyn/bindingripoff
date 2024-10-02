using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float regenAmount = 0; //% of health
    public float damage = 0; //base dmg increase
    public float reloadSpeed = 0;
    public float defense = 0; //dmg resistance % increase
    public float evasion = 0;
    public float movementSpeed = 0; //only to apply buff/debuff
    public float luck = 0;

    public List<string> upgrades;

    public bool regenAmountCapped = false;

    public int baseHealthUpgrade = 10;
    private int totalBonusHealthUpgrade = 0; //increases on every max hp upgrade
    public int extraHPUpgrade = 5;

    public float regenAmountIncreasePerUpgrade = 0.005f;
    public float regenAmountCap = 0.1f;

    private void Start()
    {
        //TODO: add these in instructor
        upgrades.Add("Health");
        //upgrades.Add("Health Regeneration");
        upgrades.Add("Damage");
        upgrades.Add("Fire Rate");
        upgrades.Add("Defense");
        upgrades.Add("Evasion");
        upgrades.Add("Movement Speed");
        //upgrades.Add("Luck");
        upgrades.Add("IFrames");
        upgrades.Add("Health Pickup");
        
    }
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
