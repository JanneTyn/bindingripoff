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
        int projectile_count = 2;
        float spread_value = 45f;

        for (int i = 0; i < projectile_count; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        //var lSpreadDir = Quaternion.AngleAxis(-15, Vector3.forward) * direction;
        //var rSpreadDir = Quaternion.AngleAxis(15, Vector3.forward) * direction;

        float spread_angle = 2 * spread_value / (projectile_count - 1);
        
        for (int i = 0; i < projectiles.Count; i++){
            var newDirection =  Quaternion.AngleAxis((-spread_value + spread_angle * i) / 2, Vector3.forward) * direction;
            projectiles[i].Initialize(newDirection, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        }
    }
}