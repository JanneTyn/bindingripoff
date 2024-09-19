using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles initalization, velocity, damage, etcetera. for projectiles
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private bool shotByPlayer;
    private float damage;
    /// <summary>
    /// Initialize this projectile
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="projectileSpeed"></param>
    /// <param name="maxLifetime"></param>
    public void Initialize(Vector2 direction, float projectileSpeed, float maxLifetime, bool _shotByPlayer, float _damage, GameObject ignore)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignore.GetComponent<Collider2D>());
        GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        shotByPlayer = _shotByPlayer;
        damage = _damage;
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO collision checks

        if(collision.gameObject.TryGetComponent<IDamageable>(out var damageInterface))
        {
            if(shotByPlayer)
            {
                if(damageInterface is not Player)
                {
                    damageInterface.TakeDamage(damage);
                }
            }
            else
            {
                if(damageInterface is not EnemyBase)
                {
                    damageInterface.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
