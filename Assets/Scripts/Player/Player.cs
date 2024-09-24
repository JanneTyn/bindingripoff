using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Player main class
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour, IDamageable
{
    // testing stats for now
    /// TODO get stats from leveling component or whatever
    [Header("Testing stats")]
    public float maxHealthIncrease;
    [Range(0, 200)] public float fireRate;
    [Range(0, 200)] public float damageIncrease;
    [Range(0, 200)] public float moveSpeedIncrease;
    [Range(0, -30)] public float armor;
    [Range(0, 30)] public float dodge;
    [Range(0, -50)] public float dodgeCooldownDecrease;

    [Space(20)]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float baseDodgeCooldown;
    [SerializeField] private float dodgeVelocity;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    private float movementSmoothing = .05f;
    private float shootTimer, dodgeTimer;
    private bool dodging;
    private Vector2 movementDirection, shootDirection;
    private Vector2 refVelocity = Vector2.zero; //for�SmoothDamp
    private new Rigidbody2D rigidbody;
    private Animator animator;

    #region Input

    private PlayerInput playerInput; //literally only to check control scheme, refactor this
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

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        maxHealth = 100 + maxHealthIncrease;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        movementDirection = inputActions.Gameplay.Movement.ReadValue<Vector2>();
        shootDirection = inputActions.Gameplay.ShootDirection.ReadValue<Vector2>();
        if (inputActions.Gameplay.Shoot.ReadValue<float>() > 0.5f) Shoot(); //ew, refactor
        shootTimer += Time.deltaTime; //shoot timer tick
        dodgeTimer += Time.deltaTime; //dodge timer tick

        animator.SetFloat("MovementX", movementDirection.x);
        animator.SetFloat("MovementY", movementDirection.y);
    }

    private void FixedUpdate()
    {
        if(!dodging)
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, movementDirection * (baseMoveSpeed * PercentageToMultiplier(moveSpeedIncrease)), ref refVelocity, movementSmoothing);
    }

    private void Shoot()
    {
        //fire rate check
        if(shootTimer >= (1f / (currentWeapon.fireRate * PercentageToMultiplier(fireRate))))
        {
            shootTimer = 0f; // reset shot timer

            if(playerInput.currentControlScheme == "Gamepad") //controller aim
            {   
                shootDirection = shootDirection.normalized;
            }
            else //m&k aim
            {
                var shootDir = ((Vector2)Camera.main.ScreenToWorldPoint(shootDirection) - (Vector2)transform.position).normalized;
                currentWeapon.Shoot(gameObject, shootDir, true, PercentageToMultiplier(damageIncrease));
            }
        }
    }

    private void Dash()
    {
        //TODO effects
        if(dodgeTimer >= baseDodgeCooldown * PercentageToMultiplier(dodgeCooldownDecrease) && movementDirection != Vector2.zero)
        {
            dodgeTimer = 0f;
            StartCoroutine(DodgeRoutine());
        }
    }

    

    public void TakeDamage(float damageAmount)
    {
        //TODO death effects etc.
        
        if(Random.Range(0, 100) < dodge) //dodge success
        {
            Debug.Log("Player dodged");
        }
        else //take dmg
        {
            Debug.Log("Player took " + damageAmount * PercentageToMultiplier(armor) + " damage");
            currentHealth = Mathf.Clamp(currentHealth - damageAmount * PercentageToMultiplier(armor), 0f, maxHealth);
            if (currentHealth == 0f) Death();
        }
    }

    private void Death()
    {
        //TODO death screen etc.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Turn percentages to multipliers for easy increases
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns></returns>
    private float PercentageToMultiplier(float percentage) => (percentage / 100f) + 1f;

    /// <summary>
    /// Coroutine for dodging, WIP
    /// </summary>
    /// <returns></returns>
    private IEnumerator DodgeRoutine()
    {
        rigidbody.velocity = movementDirection * dodgeVelocity;
        dodging = true;
        yield return new WaitForSeconds(0.1f);
        dodging = false;
    }
}
