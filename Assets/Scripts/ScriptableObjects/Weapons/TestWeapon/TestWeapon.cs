using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TestWeapon", menuName = "Scriptable Object/Weapons/TestWeapon")]
public class TestWeapon : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
