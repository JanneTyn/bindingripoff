using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    public enum PlayerVisualWeaponType
    {
        Pistol = 0,
        Rifle,
        Shotgun,
        Flamer,
        Rocket
    }
    public PlayerVisualWeaponType playerVisualWeaponType;
    public Rarity rarity;
    public float fireRate = 5f;
    public float damage = 1f;
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
    /// <param name="randomSpread"></param>
    /// <param name="damageMultiplier"></param>
    /// <param name="projectilesPerShot"></param>
    /// <param name="projectileSpread"></param>
    public abstract void Shoot(GameObject origin, Vector2 direction, bool shotByPlayer, float damageMultiplier = 1f);
}