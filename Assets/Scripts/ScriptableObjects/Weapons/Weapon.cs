using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public float fireRate;
    public float damage;
    public float projectileSpeed;
    public float projectileLifetime;
    public GameObject projectilePrefab;

    public abstract void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f);
}
