using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all enemies
/// All enemies inherit this, shared functionality goes here
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private float currentHealth, maxHealth;
    protected Vector2 movementVector;

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);

        if (currentHealth == 0f) Death();
    }

    /// <summary>
    /// Kill this enemy
    /// </summary>
    protected void Death()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    protected void FixedUpdate()
    {
        animator.SetFloat("MovementX", movementVector.normalized.x);
        animator.SetFloat("MovementY", movementVector.normalized.y);

        rigidbody.velocity = movementVector;
    }
}
