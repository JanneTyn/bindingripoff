using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cross Pattern Weapon", menuName = "Scriptable Object/Weapons/Enemy Weapons/Cross Pattern")]
public class CrossPatternWeapon : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        List<Projectile> projectiles = new();

        //instantiate projectiles and initialize them to fly in directions of a cross / t pattern

        for(int i = 0; i < 4; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        projectiles[0].Initialize(Vector2.up, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[1].Initialize(Vector2.down, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[2].Initialize(Vector2.left, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[3].Initialize(Vector2.right, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}
