using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    public int healthDropChance = 10;
    public int weaponDropChance = 3;
    public static float healthChanceMultiplier = 1.0f;
    public static float weaponChanceMultiplier = 1.0f;

    private void Awake() { DontDestroyOnLoad(gameObject); }

    public void RollEnemyDrop(Transform enemyPos)
    {
        int weapondrop = Random.Range(0, 101);
        if (weapondrop < (weaponDropChance * weaponChanceMultiplier))
        {
            var weaponPickup = Resources.Load("WeaponPickup") as GameObject;
            var go = Instantiate(weaponPickup, enemyPos.position, Quaternion.identity);
            go.GetComponent<WeaponPickup>().RandomizeWeapon();
        }
        else
        {
            int pickupChance = Random.Range(0, 101);
            if (pickupChance < (healthDropChance * healthChanceMultiplier))
            {
                var healthPickup = Resources.Load("Pickups/HealthPickUp") as GameObject;
                Instantiate(healthPickup, enemyPos.position, Quaternion.identity);
            }
        }
    }
}
