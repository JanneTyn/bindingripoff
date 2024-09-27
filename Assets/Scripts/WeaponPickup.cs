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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.sprite;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.CompareTag("Player"))
        {
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
        
        var rarity = Random.Range(0, 101);
        if(rarity > 90) //rare
        {
            weapon = rareWeapons[Random.Range(0, rareWeapons.Length)] as Weapon;
        }
        else if(rarity > 60) //uncommon
        {
            weapon = unCommonWeapons[Random.Range(0, unCommonWeapons.Length)] as Weapon;
        }
        else //common
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
        else spriteRenderer.color = Color.gray;
    }
}
