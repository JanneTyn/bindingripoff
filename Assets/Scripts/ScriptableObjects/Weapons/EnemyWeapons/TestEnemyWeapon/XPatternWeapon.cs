using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New X Pattern Weapon", menuName = "Scriptable Object/Weapons/Enemy Weapons/X Pattern")]
public class XPatternWeapon : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f, int projectilesPerShot = 1, float projectileSpread = 0, float randomSpread = 0f)
    {
        List<Projectile> projectiles = new();

        //instantiate projectiles and initialize them to fly in directions of a cross / t pattern

        for (int i = 0; i < 4; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        projectiles[0].Initialize(new Vector2(0.5f, 0.5f), projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[1].Initialize(new Vector2(0.5f, -0.5f), projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[2].Initialize(new Vector2(-0.5f, 0.5f), projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[3].Initialize(new Vector2(-0.5f, -0.5f), projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
