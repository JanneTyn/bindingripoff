using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Scriptable Object/Weapons/Shotgun")]
public class Shotgun : Weapon
{
    public int projectileCount;
    public int projectileSpread;

    public override void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f)
    {

        //Create defined amount of projectiles and spread their directions
        //in a fan by 'projectileSpread'

        List<Projectile> projectiles = new();


        for (int i = 0; i < projectileCount; i++)
        {
            projectiles.Add(Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity).GetComponent<Projectile>());
        }

        float spread_angle = 2 * projectileSpread / (projectileCount - 1);
        
        for (int i = 0; i < projectiles.Count; i++){
            var newDirection =  Quaternion.AngleAxis((-projectileSpread + spread_angle * i) / 2, Vector3.forward) * direction;
            projectiles[i].Initialize(newDirection, projectileSpeed, projectileLifetime, shotByPlayer, damage * damageMultiplier, origin);
        }
    }
}