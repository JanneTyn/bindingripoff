using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rocket Launcher", menuName = "Scriptable Object/Weapons/RocketLauncher")]
public class RocketLauncher : Weapon
{
    public float aoeRadius;

    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        var projectile = Instantiate(projectilePrefab, origin.transform.position + Vector3.up + (Vector3)direction * 1.2f, Quaternion.identity);
        projectile.GetComponent<ExplosiveProjectile>().Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin, aoeRadius);
    }
}
