using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Copy of 'Projectile' but creates AoE damage hitbox instead
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ExplosiveProjectile : MonoBehaviour
{
    private bool shotByPlayer;
    private float radius;
    private float damage;
    private new Rigidbody2D rigidbody;
    private Vector2 velocityVector;

    /// <summary>
    /// Initialize this projectile
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="projectileSpeed"></param>
    /// <param name="maxLifetime"></param>
    public void Initialize(Vector2 direction, float projectileSpeed, float maxLifetime, bool _shotByPlayer, float _damage, GameObject ignore, float _radius)
    {
        if (ignore.CompareTag("Player")) GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Player");
        else GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Enemy");

        Physics2D.IgnoreLayerCollision(6, 6); //temporary fix, projectiles ignore collisions with eachother
        rigidbody = GetComponent<Rigidbody2D>();
        velocityVector = direction * projectileSpeed;
        radius = _radius;
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
        Destroy(gameObject, 0.1f);
        CameraController.instance.StartShake(0.2f, 0.2f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true; //activate child AoE sprite
        GetComponent<SpriteRenderer>().enabled = false; // deactivate projectile sprite
        rigidbody.constraints = RigidbodyConstraints2D.FreezePosition; // freeze projectile

        //fix localScale caused by parent projectile size, set radius
        transform.GetChild(0).localScale = 
            new Vector3(radius * 2f * (1f / transform.localScale.x),
                        radius * 2f * (1f / transform.localScale.y),
                        radius * 2f * (1f / transform.localScale.z));

        //AoE damage
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out var damageInterface))
            {
                if (shotByPlayer)
                {
                    if (damageInterface is not Player)
                    {
                        damageInterface.TakeDamage(damage);
                    }
                }
                else
                {
                    if (damageInterface is not Enemy)
                    {
                        damageInterface.TakeDamage(damage);
                    }
                }
            }
        }
    }
}