using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Player main class
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour, IDamageable
{
    #region Test upgrades
    // testing stats for now
    /// TODO get stats from leveling component or whatever
    [Header("Testing upgrades")]
    public float maxHealthIncrease;
    [Range(0, 200)] public float fireRate;
    [Range(0, 200)] public float damageIncrease;
    [Range(0, 200)] public float moveSpeedIncrease;
    [Range(0, -30)] public float armor;
    [Range(0, 30)] public float dodge;
    [Range(0, -50)] public float dodgeCooldownDecrease;
    [Range(0, 50)] public float iFramesLengthIncrease;
    [Range(0, 10)] public float healthRegen;
    #endregion

    [Space(20)]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float baseDodgeCooldown;
    [SerializeField] private float dodgeVelocity;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private float maxHealth;
    [HideInInspector] public float currentHealth { get; private set; }
    
    private float movementSmoothing = .05f;
    private float shootTimer, dodgeTimer;
    private bool dodging, iFramesActive;
    private bool corruptCRActive;
    public bool healthRegenEnabled;
    private Vector2 movementDirection, shootDirection;
    private Vector2 refVelocity = Vector2.zero; //for�SmoothDamp

    private new Rigidbody2D rigidbody;
    private SpriteTint tinter;
    private Animator animator;
    private StatDisplay UI;

    #region Input

    private PlayerInput playerInput; //literally only to check control scheme, refactor this
    public InputActionsAsset inputActions { get; private set; }

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
        DontDestroyOnLoad(gameObject);
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        tinter = GetComponent<SpriteTint>();

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

        if (!corruptCRActive)
        {
            StartCoroutine(CorruptionDamageDelay());
        }
    }

    private void FixedUpdate()
    {
        if(!dodging)
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, movementDirection * (baseMoveSpeed * PercentageToMultiplier(moveSpeedIncrease)), ref refVelocity, movementSmoothing);
        if (healthRegenEnabled)
        {
            Heal(healthRegen * Time.fixedDeltaTime); //based on deltatimescale 0.02
        }
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

    public void Heal(float healAmountPercentage)
    {
        float healAmount = maxHealth * (healAmountPercentage / 100);
        currentHealth += healAmount;
        Debug.Log("Player healed for " + healAmount);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        //jos iframet aktivoituna, älä ota damagea -> return
        //jos ei aktivoituna, ota damagea ja aloita iframet
        if (iFramesActive) return;
        else
        {
            StartCoroutine(IFrameRoutine());
        }
        
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
            tinter.FlashColor(SpriteTint.DamageRed);
            CameraController.instance.StartShake(0.2f, 0.2f);
        }
    }

    public void TakeCorruptionDamage(float damageAmount)
    {
        Debug.Log("Player took " + damageAmount * PercentageToMultiplier(armor) + " corruption damage");
        currentHealth = Mathf.Clamp(currentHealth - damageAmount * PercentageToMultiplier(armor), 0f, maxHealth);
        if (currentHealth == 0f) Death();
    }
    private void Death()
    {
        //TODO death screen etc.

        //UI = GameObject.Find("Canvas").GetComponent<StatDisplay>();
        GameUIController.instance.deathUI.SetActive(true);

        Time.timeScale = 0f;
        animator.SetBool("paused", true);
    }

    /// <summary>
    /// Equip player with this weapon
    /// </summary>
    /// <param name="_weapon"></param>
    public void EquipWeapon(Weapon _weapon, WeaponPickup pickupComponent)
    {
        //TODO weapon slot system, drop current weapon
        Weapon oldWeapon = currentWeapon;
        currentWeapon = _weapon;
        pickupComponent.UpdateWeaponPickup(oldWeapon);
    }

    public Weapon CurrentWeaponSprite() {
        return currentWeapon;
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

    /// <summary>
    /// Coroutine for player i-frames
    /// </summary>
    /// <returns></returns>
    private IEnumerator IFrameRoutine()
    {
        iFramesActive = true;
        yield return new WaitForSeconds(1f * PercentageToMultiplier(iFramesLengthIncrease));
        iFramesActive = false;
    }

    private IEnumerator CorruptionDamageDelay()
    {
        corruptCRActive = true;
        yield return new WaitForSeconds(1);
        TakeCorruptionDamage(maxHealth * (0.05f + (healthRegen / 100)) * CorruptTimer.corruptPercentage);
        corruptCRActive = false;
    }
}
