using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Scriptable Object/Weapons/Shotgun")]
public class Shotgun : Weapon
{
    public int projectilesPerShot = 3;
    public float projectileSpread = 45f;

    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {
        List<Projectile> projectiles = new();

        for (int i = 0; i < projectilesPerShot; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position + Vector3.up + (Vector3)direction * 1.2f, Quaternion.identity).GetComponent<Projectile>());
        }

        float spread_angle = 0; // by default we assume we shoot one projectile

        if (projectilesPerShot > 1){ // if we shoot more than 1 projectile, apply angle calculations (otherwise divides by 0)
            spread_angle = 2 * projectileSpread / (projectilesPerShot - 1);
        } else {
            projectileSpread = 0f; // set max_spread to 0 when shooting 1 projectile (otherwise it shoots in an unwanted angle and not at 0 degree)
        }
        
        for (int i = 0; i < projectiles.Count; i++){
            var newDirection =  Quaternion.AngleAxis((-projectileSpread + spread_angle * i) / 2, Vector3.forward) * direction;
            projectiles[i].Initialize(newDirection, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        }
    }
}