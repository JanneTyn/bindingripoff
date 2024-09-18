using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player main class
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    private InputActionsAsset inputActions;
    private Vector2 movementDirection, shootDirection;
    [SerializeField] private float debugMoveSpeed = 10f;
    [SerializeField] private float movementSmoothing = .5f;
    private Vector2 refVelocity = Vector2.zero;

    private new Rigidbody2D rigidbody;
    private Animator animator;

    #region Input
    private void Awake()
    {
        inputActions = new InputActionsAsset();
        inputActions.Enable();

        inputActions.Gameplay.Dodge.performed += context => Dash();
        inputActions.Gameplay.Shoot.performed += context => Shoot();
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
        //Shooting goes here
        
        //Get current active scheme to determine direction from
        
    }

    private void Dash()
    {
        //Dash goes here
    }

    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        movementDirection = inputActions.Gameplay.Movement.ReadValue<Vector2>();

        animator.SetFloat("MovementX", movementDirection.x);
        animator.SetFloat("MovementY", movementDirection.y);

        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, movementDirection * debugMoveSpeed, ref refVelocity, movementSmoothing);
    }

    public void TakeDamage(float damageAmount)
    {

    }
}
