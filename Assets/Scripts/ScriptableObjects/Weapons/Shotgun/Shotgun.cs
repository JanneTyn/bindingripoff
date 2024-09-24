using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Scriptable Object/Weapons/Shotgun")]
public class Shotgun : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        //Create 3 projectiles
        //Create 2 spread directions by rotating the direction vector +-15 degrees
        //Initialize projectiles to fly in adjusted directions

        List<Projectile> projectiles = new();

        for (int i = 0; i < 3; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        var lSpreadDir = Quaternion.AngleAxis(-15, Vector3.forward) * direction;
        var rSpreadDir = Quaternion.AngleAxis(15, Vector3.forward) * direction;

        projectiles[0].Initialize(direction, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[1].Initialize(lSpreadDir, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        projectiles[2].Initialize(rSpreadDir, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
    }
}