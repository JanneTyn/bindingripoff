using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon ground pickup component
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private SpriteRenderer spriteRenderer;
    private int[] commonWeaponsChance =     { 101, 101, 101, 101, 101, 101, 101, 101, 101 }; //aika turha declarointi oikeastaan
    private int[] uncommonWeaponsChance =   { 25, 50, 101, 101, 101, 101, 101, 101, 101 };
    private int[] rareWeaponsChance =       { 5, 10, 25, 50, 101, 101, 101, 101, 101 };
    private int[] epicWeaponsChance =       { 0, 2, 5, 10, 25, 50, 101, 101, 101 };
    private int[] legendaryWeaponsChance =  { 0, 0, 0, 2, 5, 10, 25, 50, 101 };

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.sprite;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.audioClipListAsset.pickupWeapon, transform.position);
            collider.transform.root.GetComponent<Player>().EquipWeapon(weapon, this);
            //Destroy(gameObject);
        }
    }

    /// <summary>
    /// Change this pickup's weapon to a different one
    /// Done when weapon is picked up by player
    /// to leave the old weapon on the ground
    /// </summary>
    public void UpdateWeaponPickup(Weapon oldWeapon)
    {
        weapon = oldWeapon;
        spriteRenderer.sprite = weapon.sprite;
        UpdateColor();
    }

    /// <summary>
    /// Randomize the weapon on this pickup
    /// </summary>
    public void RandomizeWeapon()
    {
        //TODO weighted generation
        /*
        var weapons = Resources.LoadAll("Weapons/");
        weapon = weapons[Random.Range(0, weapons.Length)] as Weapon;
        */
        var commonWeapons = Resources.LoadAll("Weapons/Common/");
        var unCommonWeapons = Resources.LoadAll("Weapons/UnCommon/");
        var rareWeapons = Resources.LoadAll("Weapons/Rare");
        var epicWeapons = Resources.LoadAll("Weapons/Epic");
        var legendaryWeapons = Resources.LoadAll("Weapons/Legendary");

        int currentLvl = TestDifficultyScaler.instance.currentLevel;
        if (currentLvl > legendaryWeapons.Length) { currentLvl = legendaryWeapons.Length; }
        
        var rarity = Random.Range(0, 100);
        if (rarity < legendaryWeaponsChance[currentLvl]) 
        {
            weapon = legendaryWeapons[Random.Range(0, legendaryWeapons.Length)] as Weapon;
        }
        else if(rarity < epicWeaponsChance[currentLvl])
        {
            weapon = epicWeapons[Random.Range(0, epicWeapons.Length)] as Weapon;
        }
        else if(rarity < rareWeaponsChance[currentLvl])
        {
            weapon = rareWeapons[Random.Range(0, rareWeapons.Length)] as Weapon;
        }
        else if (rarity < uncommonWeaponsChance[currentLvl])
        {
            weapon = unCommonWeapons[Random.Range(0, unCommonWeapons.Length)] as Weapon;
        }
        else
        {
            weapon = commonWeapons[Random.Range(0, commonWeapons.Length)] as Weapon;
        }

        UpdateColor();
    }
    private void UpdateColor()
    {
        if(!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (weapon.rarity == Weapon.Rarity.Uncommon) spriteRenderer.color = Color.green;
        else if (weapon.rarity == Weapon.Rarity.Rare) spriteRenderer.color = Color.blue;
        else if (weapon.rarity == Weapon.Rarity.Epic) spriteRenderer.color = new Color(0.8f, 0.1f, 0.8f, 1);
        else if (weapon.rarity == Weapon.Rarity.Legendary) spriteRenderer.color = new Color(1.0f, 0.6f, 0f, 1);
        else spriteRenderer.color = Color.gray;
    }
}
