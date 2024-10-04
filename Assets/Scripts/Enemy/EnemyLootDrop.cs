using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    public int healthDropChance = 10;
    public static float chanceMultiplier = 1.0f;

    private void Awake() { DontDestroyOnLoad(gameObject); }

    public void RollEnemyDrop(Transform enemyPos)
    {
        int pickupChance = Random.Range(0, 101);
        if (pickupChance < (healthDropChance * chanceMultiplier))
        {
            var healthPickup = Resources.Load("Pickups/HealthPickUp") as GameObject;
            Instantiate(healthPickup, enemyPos.position, Quaternion.identity);
        }
    }
}
