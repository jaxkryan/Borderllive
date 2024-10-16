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

    public GameObject inventory;
    OwnedActiveItem ownedActiveItem;
    private CharacterStat characterStat;
    Rigidbody2D rb;
    Animator animator;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpImpulse = 17.2f;
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
        if (!this.enabled)
        {
            // Logger.Log("PlayerController script is disabled in Awake, enabling it now.");
            this.enabled = true; // Enable the script from Awake()
        }
        ownedActiveItem = GetComponent<OwnedActiveItem>();
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
        // if (ownedActiveItem.item2 is Item7 || ownedActiveItem.item1 is Item7)
        // {
        //     Debug.Log("It suppose to revive");
        //     return;
        // }
        // else{
        //     Debug.Log("NOPE");
        // }
        // Xóa dữ liệu khi người chơi chết
        ClearPlayerData();

        // Có thể thêm mã xử lý cái chết của người chơi ở đây
        Debug.Log("Player has died. Player data cleared.");
        // Load the Game Over screen
        SceneManager.LoadScene("GameOver_Screen");
    }
    void Start()
    {
        LoadPlayerData();

    }

    void Update()
    {

        if (isDashing) { return; }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle the panel's active state
            if (inventory != null)
            {
                inventory.SetActive(!inventory.activeSelf);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Logger.Log("OnMove called");
        moveInput = context.ReadValue<Vector2>();
        Logger.Log("OnMove: " + moveInput);
        Logger.Log("On move at stage " + SceneManager.GetActiveScene().name);
        Logger.Log("Is GameObject active after room change: " + gameObject.activeSelf);
        Logger.Log("Is script enabled after room change: " + this.enabled);
        this.enabled = true;
        Logger.Log("PlayerController instance count: " + FindObjectsOfType<PlayerController>().Length);
        Logger.Log("Is Rigidbody kinematic: " + rb.isKinematic);
        Logger.Log("Rigidbody velocity: " + rb.velocity);



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

    private void FixedUpdate()
    {
        //Logger.Log("FixedUpdate called");
        if (isDashing) { return; }
        if (!damageable.LockVelocity)
        {
            //Logger.Log("Updating velocity: " + moveInput * CurrentSpeed);
            rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
            //Logger.Log("Character velocity: " + rb.velocity);
        }
        else
        {
            //Logger.Log("Velocity locked: " + damageable.LockVelocity);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Update running state based on movement
        if (moveInput == Vector2.zero)
        {
            IsRunning = false;
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

            jumpImpulse += 0.5f;
        }
        else if (context.canceled)
        {
            jumpImpulse -= 0.5f;
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

    public string skill1Trigger;

    public void OnSkill1(InputAction.CallbackContext context)
    {
        Debug.Log("cast skill 1");
        if (context.started)
        {
            animator.SetTrigger(skill1Trigger);
        }
    }
    public string skill2Trigger;
    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(skill2Trigger);
        }
    }
    public string skill3Trigger;
    public void OnSkill3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(skill3Trigger);
        }
    }
    //trigger with q
    public string item1Trigger;
    public void OnItem1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Item currentItem = ownedActiveItem.item1;
            if (currentItem == null) return;
            GameObject Item1 = GameObject.Find("Item1");
            SpellCooldown sp = Item1.GetComponent<SpellCooldown>();
            sp.cooldownTime = currentItem.cd;
            if (sp != null)
            {
                if (sp.UseSpell())
                {
                    // Check if the current item is not null
                    if (currentItem != null)
                    {
                        // Activate the item's effect
                        currentItem.Activate();

                        // Set the animator trigger
                        animator.SetTrigger(item1Trigger);
                    }
                }
            }
        }
    }
    public string item2Trigger;
    public void OnItem2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Item currentItem = ownedActiveItem.item2;
            if (currentItem == null) return;
            GameObject Item2 = GameObject.Find("Item2");
            SpellCooldown sp = Item2.GetComponent<SpellCooldown>();
            sp.cooldownTime = currentItem.cd;
            if (sp != null)
            {
                if (sp.UseSpell())
                {
                    // Check if the current item is not null
                    if (currentItem != null)
                    {
                        // Activate the item's effect
                        currentItem.Activate();


                        // Set the animator trigger
                        animator.SetTrigger(item1Trigger);
                    }
                }
            }

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
            berserkGauge.DecreaseProgress(8f);
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

    public static void ClearPlayerData()
    {
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("MaxHealth");
        PlayerPrefs.DeleteKey("Shield");
        PlayerPrefs.DeleteKey("XP");
        PlayerPrefs.DeleteKey("Souls");
        PlayerPrefs.DeleteKey("ActivePowerups");
        PlayerPrefs.DeleteKey("Item1");
        PlayerPrefs.DeleteKey("Item2");
        timer = FindObjectOfType<Timer>();
        timer.ResetTime();
        PlayerPrefs.DeleteKey("EnemyXP");

        PlayerPrefs.Save();
    }

    private CurrencyManager currencyManager;
    private XPTracker xPTracker;
    private static Timer timer;
    public void SavePlayerState()
    {
        timer = FindObjectOfType<Timer>();
        timer.SaveTime();
        CharacterStat characterStat = GetComponent<CharacterStat>();
        // Save player health
        PlayerPrefs.SetInt("Health", damageable.Health);
        PlayerPrefs.SetInt("MaxHealth", damageable.MaxHealth);
        Debug.Log("c mau: " + PlayerPrefs.GetInt("Health"));
        Debug.Log("max mau: " + PlayerPrefs.GetInt("MaxHealth"));
        PlayerPrefs.SetFloat("Shield", characterStat.Shield);


        // Save player XP
        PlayerPrefs.SetInt("XP", xPTracker.CurrentXP);

        // Save player souls (currency) - cong 30 la cdj the cac ban??
        if (currencyManager != null)
        {
            PlayerPrefs.SetInt("Souls", currencyManager.currentAmount); // Assuming CurrentMoney tracks souls
        }

        // Save active power-ups
        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        if (ownedPowerups != null)
        {
            ownedPowerups.SavePowerups();
        }
        OwnedActiveItem ownedItems = GetComponent<OwnedActiveItem>();
        if (ownedItems != null)
        {
            ownedItems.SaveItems();
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
        if (PlayerPrefs.HasKey("Shield"))
        {
            CharacterStat characterStat = GetComponent<CharacterStat>();
            characterStat.IncreaseShield(PlayerPrefs.GetFloat("Shield"));
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
        OwnedActiveItem ownedItems = GetComponent<OwnedActiveItem>();
        if (ownedItems != null)
        {
            ownedItems.LoadItems();
        }

        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        if (ownedPowerups != null)
        {
            ownedPowerups.LoadPowerups();
        }
    }
    private void OnApplicationQuit()
    {
        LevelController.ResetStaticData();
        ClearPlayerData();

        Debug.Log("Player data cleared on application quit.");
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
        dashingPower += dashingCd * 0.1f;
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
        // Debug.Log("bs rg inc: " + berserkGauge.berserkRegenIncrease);
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
        Debug.Log($"Old MaxHealth: {damageable.MaxHealth}, New MaxHealth: {newMaxHealth}");
        damageable.MaxHealth = newMaxHealth;

        Debug.Log("Earth 1 active!: " + damageable.MaxHealth);
        damageable.Health += (int)(damageable.Health * value);
        // Debug.Log("max health: " + damageable.MaxHealth);
        // Debug.Log("c health " + damageable.Health);
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
