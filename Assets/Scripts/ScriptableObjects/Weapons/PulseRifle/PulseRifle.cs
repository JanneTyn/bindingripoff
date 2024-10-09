using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pulse Rifle", menuName = "Scriptable Object/Weapons/Pulse Rifle")]

public class PulseRifle : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position + Vector3.up + (Vector3)direction * 1.2f, Quaternion.identity);
        projectile.GetComponent<BouncingProjectile>().Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
