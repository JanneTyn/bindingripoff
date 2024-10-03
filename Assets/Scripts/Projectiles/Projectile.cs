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
    private new Rigidbody2D rigidbody;
    private Vector2 velocityVector;
    private new Collider2D collider;
    private bool collide = false;

    /// <summary>
    /// Initialize this projectile
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="projectileSpeed"></param>
    /// <param name="maxLifetime"></param>
    public void Initialize(Vector2 direction, float projectileSpeed, float maxLifetime, bool _shotByPlayer, float _damage, GameObject ignore)
    {
        collider = GetComponent<Collider2D>();

        if (ignore.CompareTag("Player")) collider.excludeLayers = LayerMask.GetMask("Player");
        else collider.excludeLayers = LayerMask.GetMask("Enemy");
        Physics2D.IgnoreLayerCollision(6, 6); //temporary fix, projectiles ignore collisions with eachother
        collide = true;

        rigidbody = GetComponent<Rigidbody2D>();
        velocityVector = direction * projectileSpeed;
        shotByPlayer = _shotByPlayer;
        damage = _damage;
        Destroy(gameObject, maxLifetime);
    }
    private void Update()
    {
        rigidbody.velocity = velocityVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collide) return;

        if(collision.transform.root.TryGetComponent<IDamageable>(out var damageInterface))
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
                if(damageInterface is not Enemy)
                {
                    damageInterface.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
