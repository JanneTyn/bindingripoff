using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Weapon", menuName = "Scriptable Object/Weapons/BasicWeapon")]
public class BasicWeapon : Weapon
{
    public float randomSpread = 0f;

    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position + (Vector3)direction * 1.2f, Quaternion.identity);
        float spreadRNG = UnityEngine.Random.Range(-0.5f, 0.5f); // if randomSpread is 5f, possible spread range is (-2.5f // 2.5f)
        var newDirection =  Quaternion.AngleAxis(randomSpread * spreadRNG, Vector3.forward) * direction; // apply some randomness to the projectile direction

        projectile.GetComponent<Projectile>().Initialize(newDirection, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
