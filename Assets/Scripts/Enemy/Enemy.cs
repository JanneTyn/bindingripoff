using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all enemies
/// All enemies inherit this, shared functionality goes here
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteTint))]
public class Enemy : MonoBehaviour, IDamageable
{
    protected bool aiActive = false;
    [SerializeField] protected float movementSpeed;
    [HideInInspector] public Room room;
    [HideInInspector] public EnemyLootDrop enemyDrop;

    private new Rigidbody2D rigidbody;
    private Animator animator;

    public float currentHealth; // { get; private set; }
    [SerializeField] private SpriteTint tint;
    [SerializeField] public float maxHealth;
    public float baseMaxHealth;
    [SerializeField] public float maxHealthMultiplier;
    [SerializeField] public float damageMultiplier;
    protected Vector2 movementVector = Vector2.zero;
    private PlayerLeveling playerLeveling;

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
        tint.FlashColor(SpriteTint.DamageRed);
        if (currentHealth == 0f) Death();
    }

    /// <summary>
    /// Kill this enemy
    /// </summary>
    protected void Death()
    {
        playerLeveling.IncreaseXP();
        if (room) room.EnemyKilled();
        enemyDrop.RollEnemyDrop(this.transform);
        Destroy(gameObject);
    }
    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerLeveling = GameObject.Find("TestPlayer").GetComponent<PlayerLeveling>();
        enemyDrop = GameObject.Find("EnemyDrops").GetComponent<EnemyLootDrop>();
        tint = GetComponent<SpriteTint>();
        maxHealth = baseMaxHealth * maxHealthMultiplier;
        currentHealth = maxHealth;
        StartCoroutine(ActivateAI());
    }

    private IEnumerator ActivateAI()
    {
        var rend = GetComponent<SpriteRenderer>();
        var col = new Color(1, 1, 1, 0.7f);
        rend.color = col;

        yield return new WaitForSeconds(1f);
        aiActive = true;
        rend.color = Color.white;
    }

    protected void FixedUpdate()
    {
        if (!aiActive) return;
        animator.SetFloat("MovementX", movementVector.normalized.x);
        animator.SetFloat("MovementY", movementVector.normalized.y);

        rigidbody.velocity = movementVector;
    }
}