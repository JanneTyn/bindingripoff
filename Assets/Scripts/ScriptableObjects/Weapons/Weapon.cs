using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare
    }
    public Rarity rarity;
    public float fireRate;
    public float damage;
    public float projectileSpeed;
    public float projectileLifetime;
    public GameObject projectilePrefab;
    public Sprite sprite;

    /// <summary>
    /// Fire this weapon. Abstract, so implement this in each child of 'Weapon'.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="shotByPlayer"></param>
    /// <param name="damageMultiplier"></param>
    public abstract void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f);
}