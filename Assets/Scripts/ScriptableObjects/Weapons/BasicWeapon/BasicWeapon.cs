using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Weapon", menuName = "Scriptable Object/Weapons/BasicWeapon")]
public class BasicWeapon : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
