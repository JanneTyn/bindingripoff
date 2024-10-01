using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Scriptable Object/Weapons/Shotgun")]
public class Shotgun : Weapon
{
    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f, int projectilesPerShot = 1, float projectileSpread = 0, float randomSpread = 0f)
    {
        List<Projectile> projectiles = new();
        float spread_value = 45f;

        for (int i = 0; i < projectilesPerShot; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        float spread_angle = 0; // by default we assume we shoot one projectile

        if (projectilesPerShot > 1){ // if we shoot more than 1 projectile, apply angle calculations (otherwise divides by 0)
            spread_angle = 2 * spread_value / (projectilesPerShot - 1);
        } else {
            spread_value = 0f; // set max_spread to 0 when shooting 1 projectile (otherwise it shoots in an unwanted angle and not at 0 degree)
        }
        
        for (int i = 0; i < projectiles.Count; i++){
            var newDirection =  Quaternion.AngleAxis((-spread_value + spread_angle * i) / 2, Vector3.forward) * direction;
            projectiles[i].Initialize(newDirection, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        }
    }
}