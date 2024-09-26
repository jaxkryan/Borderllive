using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    BerserkGauge berserkGauge;
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
        berserkGauge = GetComponent<BerserkGauge>();
        rb = GetComponent<Rigidbody2D>();
        ownedPowerups = GetComponent<OwnedPowerups>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        xPTracker = FindObjectOfType<XPTracker>();
        // lockAttackCollider = GetComponent<CircleCollider2D>();
        // Subscribe to the damageableDeath event
        damageable.damageableDeath.AddListener(OnPlayerDeath);

    }
    private void OnPlayerDeath()
    {

        // Xóa dữ liệu khi người chơi chết
        ClearPlayerData();

        // Có thể thêm mã xử lý cái chết của người chơi ở đây
        Debug.Log("Player has died. Player data cleared.");
        // Load the Game Over screen
        SceneManager.LoadScene("GameOver_Screen");
    }
    void Start()
    {
        LoadPlayerData(); // Khôi phục dữ liệu khi bắt đầu trò chơi

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
        if (berserkGauge._isBerserkActive)
        {
            berserkGauge.DecreaseProgress(0f);
        }
        else
        {
            berserkGauge.DecreaseProgress(10f);
        }
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
            //Debug.Log("Gia tri defIncrease: " + defIncrease);
            characterStat.Endurance += defIncrease;
            Debug.Log("Gia tri defIncrease: " + characterStat.Endurance);
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
    private float effectEndTime;

    internal void HPRegen()
    {
        // Get the player's damageable component
        Damageable damageable = GetComponent<Damageable>();

        // Check if the player's health is below 35% and the effect has not been triggered recently
        if (damageable.Health < 0.35f * damageable.MaxHealth && !isEffectTriggered)
        {
            // Set the flag to true
            isEffectTriggered = true;

            // Calculate the maximum amount of health to regenerate (20% of max health)
            int maxHealthToRegen = Mathf.RoundToInt(0.2f * damageable.MaxHealth);

            // Start regenerating health every 8 seconds
            InvokeRepeating("RegenerateHealth", 0f, 8f);

            // Calculate the end time of the effect
            effectEndTime = Time.time + 40f;

            // Wait for 5 minutes before allowing the effect to be triggered again
            Invoke("ResetEffectTriggered", 300f);
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

        // Check if the effect has ended
        if (Time.time >= effectEndTime)
        {
            // Cancel the repeating invocation
            CancelInvoke("RegenerateHealth");
        }
    }
    private void ResetEffectTriggered()
    {
        // Reset the flag
        isEffectTriggered = false;
    }

    private void ClearPlayerData()
    {
        // Xóa toàn bộ dữ liệu trong PlayerPrefs
        PlayerPrefs.DeleteAll();
        // Nếu bạn chỉ muốn xóa một khóa cụ thể
        // PlayerPrefs.DeleteKey("YourKeyName");

        // Lưu lại để đảm bảo rằng dữ liệu được xóa ngay lập tức
        PlayerPrefs.Save();
    }

    private CurrencyManager currencyManager;
    private XPTracker xPTracker;
    public void SavePlayerState()
    {
        // Save player health
        PlayerPrefs.SetInt("Health", damageable.Health);
        PlayerPrefs.SetInt("MaxHealth", damageable.MaxHealth);

        // Save player XP
        PlayerPrefs.SetInt("XP", xPTracker.CurrentXP);

        // Save player souls (currency)
        if (currencyManager != null)
        {
            PlayerPrefs.SetInt("Souls", currencyManager.currentAmount + 30); // Assuming CurrentMoney tracks souls
        }

        // Save active power-ups
        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        if (ownedPowerups != null)
        {
            string powerupsJson = ownedPowerups.SerializeActivePowerups();
            PlayerPrefs.SetString("ActivePowerups", powerupsJson);
            Debug.Log("Buff:" + powerupsJson);
        }

        // Save PlayerPrefs data
        PlayerPrefs.Save();

        Debug.Log("Saving Player Data: XP = " + xPTracker.CurrentXP + ", Souls = " + currencyManager.currentAmount);
       // Debug.Log("Saving Player Data: Health = " + damageable.Health);
    }

    public void LoadPlayerData()
    {
        Debug.Log("Loading Player Data");

        // Load player health
        if (PlayerPrefs.HasKey("Health"))
        {
           // Debug.Log("Loading Player Data: Health = " + PlayerPrefs.GetInt("Health"));
            damageable.MaxHealth = PlayerPrefs.GetInt("MaxHealth");
            damageable.Health = PlayerPrefs.GetInt("Health");
        }

        // Load player XP
        if (PlayerPrefs.HasKey("XP"))
        {
           // Debug.Log("Loading Player Data: XP = " + PlayerPrefs.GetInt("XP"));

            xPTracker.AddXP(PlayerPrefs.GetInt("XP"));
        }

        // Load player souls (currency)
        if (PlayerPrefs.HasKey("Souls"))
        {
            //Debug.Log("Loading Player Data: Soul = " + PlayerPrefs.GetInt("Souls"));

            currencyManager.SetCurrency(PlayerPrefs.GetInt("Souls")); // Assuming SetMoney sets the currency amount
        }

        // Load active power-ups
        LoadPowerups();
    }


    public void LoadPowerups()
    {
        if (PlayerPrefs.HasKey("ActivePowerups"))
        {

            string powerupsJson = PlayerPrefs.GetString("ActivePowerups");
            Debug.Log("Buff:" + powerupsJson);

            OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
            if (ownedPowerups != null)
            {
                ownedPowerups.DeserializeActivePowerups(powerupsJson);
                Debug.Log("Loaded Active Powerups");
            }
        }
    }

    internal void IncreaseAgility()
    {
          characterStat = GetComponent<CharacterStat>();
           if (characterStat == null)
           {
                Debug.LogError("CharacterStat component not found!");
               return;
           }
            float spdIncrease = characterStat.Speed * 0.1f;
            dashingPower *= 0.1f;
           //Debug.Log("Gia tri defIncrease: " + (int)defIncrease);
            characterStat.Speed += spdIncrease;
    }

    internal void ReduceBerserkPenalty(float reducePercent)
    {
        berserkGauge.berserkRegenDecrease += berserkGauge.berserkRegenDecrease * reducePercent;
        //Debug.Log("bs rg dec: " + berserkGauge.berserkRegenDecrease);
    }

    internal void IncreaseBerserkRecharge(float increasePercent)
    {
        berserkGauge.berserkRegenIncrease = increasePercent;
        Debug.Log("bs rg inc: " + berserkGauge.berserkRegenIncrease);
    }

    internal void IncreaseHp(float value)
    {
        characterStat = GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            Debug.LogError("CharacterStat component not found!");
            return;
        }
        int newMaxHealth = (int)(damageable.MaxHealth * (1 + value));
        damageable.MaxHealth = newMaxHealth;
        damageable.Health += (int)(damageable.Health * value);
    }

    //internal void IncreaseDefHighHp(float value)
    //{
    //    characterStat = GetComponent<CharacterStat>();
    //    if (characterStat == null)
    //    {
    //        Debug.LogError("CharacterStat component not found!");
    //        return;
    //    }
    //    else
    //    {
    //        float defIncrease = characterStat.BaseEndurance * value;
    //        Debug.Log("Gia tri defIncrease: " + defIncrease);
    //        characterStat.Endurance += defIncrease;
    //        Debug.Log("Gia tri defIncrease: " + characterStat.BaseEndurance);
    //    }
    //}


    internal void IncreaseDefHighHp(float value)
    {

            CharacterStat characterStat = GetComponent<CharacterStat>();
           //Debug.Log("Gia tri edurance before: " + characterStat.Endurance);
           // Debug.Log("Gia tri def before: " + characterStat.DEF);

            float defIncrease = characterStat.BaseEndurance * value;
            characterStat.Endurance += defIncrease;

            //Debug.Log("Gia tri defIncrease riu: "  + characterStat.DEF);
            //Debug.Log("Gia tri edurance riu: "  + characterStat.Endurance);
        //Debug.Log("current " + characterStat.Endurance);      
    }

    internal void IncreaseShield(float value)
    {
        float shield = value * damageable.MaxHealth;
        characterStat = GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            Debug.LogError("CharacterStat component not found!");
            return;
        }
        characterStat.IncreaseShield(shield);
    }
}
    