using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Damageable : MonoBehaviour
{
    private bool metal3Active = false;
    private bool earth2Active = false;

    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    public UnityEvent<int, int> healthChanged;
    public GameObject dropItem1;
    public GameObject dropItem2;
    Animator animator;

    public bool isStunned = false;
    public float stunDuration = 2f;
    private float stunEndTime;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return Math.Min(_health, MaxHealth);
        }
        set
        {

            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
            if (ownedPowerups != null)
            {
                if (_health <= 0.5 * MaxHealth && !metal3Active && ownedPowerups.IsPowerupActive<Metal_3>())
                {
                    //Debug.Log("metal 3 buff is active");
                    ownedPowerups.TriggerMetal3Buff();
                    metal3Active = true;
                }
                if (_health >= 0.7 * MaxHealth && !earth2Active && ownedPowerups.IsPowerupActive<Earth_2>())
                {
                    ownedPowerups.TriggerEarth2Buff();
                    earth2Active = true;
                }
                if (_health <= 0.35 * MaxHealth && ownedPowerups.IsPowerupActive<Wood_3>())
                {
                    ownedPowerups.TriggerWood3Buff();
                }

                if (_health <= 0.5 * MaxHealth && ownedPowerups.IsPowerupActive<Earth_3>())
                {
                    ownedPowerups.TriggerEarth3Buff();
                }
                //else if (_health > 0.5 * MaxHealth && metal3Active)
                //{
                //    OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
                //    if (ownedPowerups != null)
                //    {
                //        ownedPowerups.RemoveMetal3Buff(); // Remove Metal_3 buff
                //        metal3Active = false; // Reset the flag so the buff can be triggered again when health falls
                //    }
                //}
            }
            if (_health <= 0)
            {
                PlayerController playerController = GetComponent<PlayerController>();
                if (playerController != null)
                {
                    OwnedActiveItem ownedActiveItem = FindObjectOfType<OwnedActiveItem>();
                    if (ownedActiveItem.item1 is Item7 || ownedActiveItem.item2 is Item7)
                    {
                        IsAlive = true;
                        //    Item7 item7 = new Item7();
                        //    item7.Activate();

                    }
                    else IsAlive = false;
                }
                else IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    public bool IsHit
    {
        get
        {
            return animator.GetBool(AnimationStrings.isHit);
        }
        private set
        {
            animator.SetBool(AnimationStrings.isHit, value);
        }
    }

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;
    [SerializeField]
    private bool _isStun = false;
    public bool IsStun
    {
        get
        {
            return _isStun;
        }
        set
        {
            _isStun = value;
            animator.SetBool(AnimationStrings.isStun, value);
            if (_isStun)
            {
                if (IsAlive)
                {
                    LockVelocity = true;
                    stunEndTime = Time.time + stunDuration;
                    isStunned = true;
                }
            }
        }
    }
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            if (value == false)
            {
                DropWhenDeath();
                GiveSoulReward();
                damageableDeath.Invoke();

            }
        }
    }

    public int soulReward = 50;
    private CurrencyManager currencyManager;
    private void GiveSoulReward()
    {
        // Use the CurrencyManager to add souls
        if (currencyManager != null)
        {
            currencyManager.AddCurrency(soulReward); // Assuming "AddMoney" is managing souls
        }
        else
        {
            Debug.LogError("CurrencyManager instance not found!");
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Update()
    {
        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        OwnedActiveItem ownedActiveItem = FindObjectOfType<OwnedActiveItem>();
        PlayerController playerController = GetComponent<PlayerController>();
        Item7 item7 = new Item7();
        if (playerController != null)
        {
            if (ownedActiveItem != null &&
    ((ownedActiveItem.item1 is Item7 || ownedActiveItem.item2 is Item7) &&
    item7.isEnable && Health <= 0))
            {
                Debug.Log("health: " + Health);
                item7.Activate(); // Revive the character and destroy the item
            }
        }
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        if (isStunned && Time.time >= stunEndTime)
        {
            isStunned = false;
            LockVelocity = false;
            animator.SetBool(AnimationStrings.isStun, false);
        }

        if (_health > 0.5 * MaxHealth && metal3Active && characterStat != null)
        {
            if (ownedPowerups != null)
            {
                ownedPowerups.RemoveMetal3Buff(); // Remove Metal_3 buff
                metal3Active = false; // Reset the flag so the buff can be triggered again if health drops below 50%
            }
        }
        if (_health < 0.7 * MaxHealth && earth2Active && characterStat != null && ownedPowerups.IsPowerupActive<Earth_2>())
        {
            Debug.Log("hp " + _health + "max health at 70: " + 0.7 * MaxHealth);

            ownedPowerups.RemoveEarth2Buff();
            earth2Active = false;
        }
        else if (_health >= 0.7 * MaxHealth && !earth2Active && characterStat != null && ownedPowerups.IsPowerupActive<Earth_2>())
        {
            ownedPowerups.TriggerEarth2Buff();
            earth2Active = true;
        }
        //if (_health >= 0.7 * MaxHealth && !earth2Active && characterStat != null)
        //{
        //    OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();

        //    if (ownedPowerups.IsPowerupActive<Earth_2>())
        //    {
        //        ownedPowerups.TriggerEarth2Buff();

        //        earth2Active = true;
        //    }
        //}
    }
    private void Awake()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        animator = GetComponent<Animator>();
        characterStat = GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            enemyStat = GetComponent<EnemyStat>();
        }

        // Check if health is above 70% and trigger Earth_2 buff if necessary
        //if (_health >= 0.7 * MaxHealth && !earth2Active 
        //    && GetComponent<OwnedPowerups>().IsPowerupActive<Earth_2>())
        //{
        //    GetComponent<OwnedPowerups>().TriggerEarth2Buff();
        //    earth2Active = true;
        //}
    }

    private CharacterStat characterStat;
    private EnemyStat enemyStat;
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            // Calculate reduced damage based on DEF attribute
            float defense = characterStat != null ? characterStat.DEF : enemyStat.DEF; // Adjust based on actual DEF field name
            float reducedDamage = Mathf.Max(damage - Mathf.Floor(defense), 0); // Ensure damage doesn't go negative

            // Apply the reduced damage
            if (characterStat != null && characterStat.Shield > 0f)
            {
                // Apply damage to shield
                characterStat.DecreaseShield(reducedDamage);
                if (characterStat.Shield <= 0f)
                {
                    // If shield is depleted, apply remaining damage to health
                    Health -= (int)(reducedDamage - characterStat.Shield);
                }
            }
            else
            {
                // If no shield, apply damage directly to health
                Health -= (int)reducedDamage;
            }
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke((int)reducedDamage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, (int)reducedDamage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }

private void DropWhenDeath()
{
    // Generate a random number between 0 and 100
    float randomValue = UnityEngine.Random.Range(0f, 10f);

    // 10% chance to drop item 1
    if (randomValue <= 10f && dropItem1 != null)
    {
        Instantiate(dropItem1, transform.position, Quaternion.identity);
    }
    // 2% chance to drop item 2 (within the remaining 90%)
    else if (randomValue > 15f && randomValue <= 17f && dropItem2 != null)
    {
        Instantiate(dropItem2, transform.position, Quaternion.identity);
    }
    else
    {
        // No item drop
        Debug.Log("No item dropped");
    }
}

}
