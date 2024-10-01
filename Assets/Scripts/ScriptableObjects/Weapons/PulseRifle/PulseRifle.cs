using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pulse Rifle", menuName = "Scriptable Object/Weapons/Pulse Rifle")]

public class PulseRifle : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1, int projectilesPerShot = 1, float projectileSpread = 0, float randomSpread = 0)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
        projectile.GetComponent<BouncingProjectile>().Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
