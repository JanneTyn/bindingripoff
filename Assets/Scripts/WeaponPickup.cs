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
            collider.gameObject.GetComponent<Player>().EquipWeapon(weapon, this);
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
    }

    /// <summary>
    /// Randomize the weapon on this pickup
    /// </summary>
    public void RandomizeWeapon()
    {
        //TODO weighted generation
        var weapons = Resources.LoadAll("Weapons/");
        weapon = weapons[Random.Range(0, weapons.Length)] as Weapon;
    }
}
