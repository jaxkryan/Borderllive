using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    private CharacterStat characterStat;
    Rigidbody2D rb;
    Animator animator;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpImpulse = 10f;
    public float airWalkSpeed = 3f;
    TouchingDirection touchingDirection;
    OwnedPowerups ownedPowerups;
    Damageable damageable;
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public float CurrentSpeed
    {
        get
        {
            if (canMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (touchingDirection.IsGround)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                //idle
                else
                    return 0;
            }
            //cant move
            else return 0;
        }
    }

    Vector2 moveInput;
    [SerializeField]
    private Boolean _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    [SerializeField]
    private Boolean _isRunning = false;

    private Boolean _isFacingRight = true;

    public bool isFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ownedPowerups = GetComponent<OwnedPowerups>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
        // lockAttackCollider = GetComponent<CircleCollider2D>();
        // Subscribe to the damageableDeath event
        damageable.damageableDeath.AddListener(OnPlayerDeath);
    }
    private void OnPlayerDeath()
    {
        // Load the Game Over screen
        SceneManager.LoadScene("GameOver_Screen");
    }
    void Start()
    {
    }

    void Update()
    {
        if (isDashing) { return; }
    }

    private void FixedUpdate()
    {
        if (isDashing) { return; }
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Update running state based on movement
        if (moveInput == Vector2.zero)
        {
            IsRunning = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attacking);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnCastFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.castFireTrigger);
        }
    }

    public void OnLockAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.lockAttackTrigger);
        }
    }

    private float stunDuration = 2f;
    public LayerMask enemyLayers;

    //private void PerformLockAttack()
    //{
    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lockAttackCollider.transform.position, lockAttackCollider.radius, enemyLayers);
    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        Damageable enemyDamageable = enemy.GetComponent<Damageable>();
    //        if (enemyDamageable != null)
    //        {
    //            enemyDamageable.Stun(stunDuration);
    //        }
    //    }
    //}

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //TODO: cant jump when died
        if (context.started && touchingDirection.IsGround && canMove)
        {
            animator.SetTrigger(AnimationStrings.jumping);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    [Header("Dash properties")]
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCd = 1f;
    [SerializeField] private TrailRenderer trailRenderer;

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine dash direction
        float dashDirection = 0f;
        if (moveInput.x != 0)
        {
            dashDirection = moveInput.x;
        }
        else
        {
            dashDirection = isFacingRight ? -1 : 1; // Dash backward if no input
        }

        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        IsRunning = true; // Automatically enter run mode after dashing
        yield return new WaitForSeconds(dashingCd);
        canDash = true;
    }
    internal void IncreaseDef(float value)
    {
        characterStat = GetComponent<CharacterStat>();
        //Debug.Log("Gia tri value erd: " + characterStat.Endurance);
        if (characterStat == null)
        {
            Debug.LogError("CharacterStat component not found!");
            return;
        }
        float defIncrease = characterStat.BaseEndurance * value;
        //Debug.Log("Gia tri defIncrease: " + (int)defIncrease);
        characterStat.Endurance += defIncrease;
        //Debug.Log("current " + characterStat.Endurance);
    }

    internal void IncreaseDefLowHP(float value)
    {
        if (damageable.Health <= 0.5 * damageable.MaxHealth)
        {
            CharacterStat characterStat = GetComponent<CharacterStat>();
            float defIncrease = characterStat.BaseEndurance * value;
            Debug.Log("Gia tri defIncrease: " + defIncrease);
            characterStat.Endurance += defIncrease;
            //Debug.Log("Gia tri defIncrease: nat");
        }
        //Debug.Log("current " + characterStat.Endurance);      
    }

    //remove metal_3 buff when health above 50% 
    internal void DecreaseDef(float value)
    {
        characterStat = GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            Debug.LogError("CharacterStat component not found!");
            return;
        }

        // Assuming you decrease the same value added by Metal_3 buff
        float defDecrease = characterStat.BaseEndurance * value;
        //Debug.Log("mau da du, hoi lai def cu");
        characterStat.Endurance -= defDecrease;
    }

    //hpregen from wood3 buff
    private bool isEffectTriggered = false;

    internal void HPRegen()
    {
        // Check if the player's health is below 35%
        if (damageable.Health < 0.35f * damageable.MaxHealth)
        {
            // Check if the effect has not been triggered recently
            if (!isEffectTriggered)
            {
                // Set the flag to true
                isEffectTriggered = true;

                // Calculate the maximum amount of health to regenerate (20% of max health)
                int maxHealthToRegen = Mathf.RoundToInt(0.2f * damageable.MaxHealth);

                // Calculate the actual amount of health to regenerate (up to the maximum)
                int healthToRegen = Mathf.Min(maxHealthToRegen, damageable.MaxHealth - damageable.Health);

                // Regenerate health
                damageable.Heal(healthToRegen);
                Debug.Log("Generating");
                // Start regenerating health every 8 seconds
                InvokeRepeating("RegenerateHealth", 0f, 1f);

                // Wait for 5 minutes before allowing the effect to be triggered again
                Invoke("ResetEffectTriggered", 300f);
            }
        }
        else
        {
            // If the player's health is above 35%, stop regenerating health
            CancelInvoke("RegenerateHealth");
            CancelInvoke("ResetEffectTriggered");
        }
    }

    private void RegenerateHealth()
    {
        // Get the player's damageable component
        Damageable damageable = GetComponent<Damageable>();

        // Calculate the amount of health to regenerate (5% of max health)
        int healthToRegen = Mathf.RoundToInt(0.05f * damageable.MaxHealth);

        // Regenerate health
        damageable.Heal(healthToRegen);
    }

    private void ResetEffectTriggered()
    {
        // Reset the flag
        isEffectTriggered = false;
    }
}