using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player main class
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour, IDamageable
{
    private Vector2 movementDirection, shootDirection;
    private Vector2 refVelocity = Vector2.zero; //for´SmoothDamp

    private new Rigidbody2D rigidbody;
    private Animator animator;
    private PlayerInput playerInput; //literally only to check control scheme, refactor this

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float movementSmoothing = .5f;
    private float shootTimer;

    [SerializeField] private Weapon currentWeapon;

    #region Input

    private InputActionsAsset inputActions;
    private void Awake()
    {
        inputActions = new InputActionsAsset();
        inputActions.Enable();

        inputActions.Gameplay.Dodge.performed += context => Dash();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion

    private void Shoot()
    {
        //fire rate check
        if(shootTimer >= (1f / currentWeapon.fireRate))
        {
            shootTimer = 0f; // reset shot timer

            if(playerInput.currentControlScheme == "Gamepad") //controller aim
            {   
                shootDirection = shootDirection.normalized;
            }
            else //m&k aim
            {
                var shootDir = ((Vector2)Camera.main.ScreenToWorldPoint(shootDirection) - (Vector2)transform.position).normalized;
                currentWeapon.Shoot(gameObject, shootDir, true);
            }
        }
        
    }

    private void Dash()
    {
        //TODO dash goes here
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        movementDirection = inputActions.Gameplay.Movement.ReadValue<Vector2>();
        shootDirection = inputActions.Gameplay.ShootDirection.ReadValue<Vector2>();
        if (inputActions.Gameplay.Shoot.ReadValue<float>() > 0.5f) Shoot(); //ew, refactor
        shootTimer += Time.deltaTime; //shot timer tick

        animator.SetFloat("MovementX", movementDirection.x);
        animator.SetFloat("MovementY", movementDirection.y);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, movementDirection * moveSpeed, ref refVelocity, movementSmoothing);
    }

    public void TakeDamage(float damageAmount)
    {
        //TODO player damage
    }
}
