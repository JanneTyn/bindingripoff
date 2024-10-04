using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BouncingProjectile : MonoBehaviour
{
    private bool shotByPlayer;
    private float damage;
    private new Rigidbody2D rigidbody;
    private Vector2 velocityVector;
    private float speed;
    private bool initialized;


    public void Initialize(Vector2 direction, float projectileSpeed, float maxLifetime, bool _shotByPlayer, float _damage, GameObject ignore)
    {
        transform.up = direction;
        if (ignore.CompareTag("Player")) GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Player");
        else GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Enemy");
        rigidbody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(6, 6);
        velocityVector = direction * projectileSpeed;
        speed = projectileSpeed;
        shotByPlayer = _shotByPlayer;
        damage = _damage;
        Destroy(gameObject, maxLifetime);
        initialized = true;
    }

    private void Update()
    {
        rigidbody.velocity = velocityVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!initialized) return;
        //velocityVector *= Vector2.Dot(velocityVector, collision.contacts[0].normal);
        velocityVector = Vector2.Reflect(velocityVector.normalized, collision.contacts[0].normal) * speed;


        if (collision.transform.root.TryGetComponent<IDamageable>(out var damageInterface))
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
