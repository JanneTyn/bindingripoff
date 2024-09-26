using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : Enemy
{
    [SerializeField] private float smoothing;
    private Transform player;
    private Vector2 refVelocity = Vector2.zero; //for SmoothDamp

    private void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        var playerDirection = (player.position - transform.position).normalized;
        movementVector = Vector2.SmoothDamp(movementVector, playerDirection * movementSpeed, ref refVelocity, smoothing);
        base.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<IDamageable>(out var dmg))
        {
            if(dmg is Player)
            {
                dmg.TakeDamage(20);
                Death();
            }
        }
    }
}
