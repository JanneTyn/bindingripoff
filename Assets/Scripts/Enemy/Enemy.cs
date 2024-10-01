using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all enemies
/// All enemies inherit this, shared functionality goes here
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected float movementSpeed;
    [HideInInspector] public Room room;

    private new Rigidbody2D rigidbody;
    private Animator animator;

    [SerializeField] public float currentHealth; // { get; private set; }
    [SerializeField] private float maxHealth;
    [SerializeField] public float maxHealthMultiplier;
    protected Vector2 movementVector = Vector2.zero;
    private PlayerLeveling playerLeveling;

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
        playerLeveling.IncreaseXP();
        room.EnemyKilled();
        Destroy(gameObject);
    }

    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxHealth = 100f * maxHealthMultiplier;
        currentHealth = maxHealth;
        playerLeveling = GameObject.Find("TestPlayer").GetComponent<PlayerLeveling>();
    }

    protected void FixedUpdate()
    {
        animator.SetFloat("MovementX", movementVector.normalized.x);
        animator.SetFloat("MovementY", movementVector.normalized.y);

        rigidbody.velocity = movementVector;
    }
}