using System;
using System.Collections;
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
    public UnityEvent<int> MaxHealthChanged;
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
            // Debug.Log("MaxHealth changed to: " + _maxHealth);
            MaxHealthChanged?.Invoke(_maxHealth);
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

        if (playerController != null)
        {
            Item7 item7 = ScriptableObject.CreateInstance<Item7>();
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
        if (MaxHealthChanged == null)
            MaxHealthChanged = new UnityEvent<int>();
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
                Debug.Log("I HAVE SHIELDDDDDDD");
                // Apply damage to shield
                float remainDmg = characterStat.DecreaseShield(reducedDamage);
                if (remainDmg > 0f)
                {
                    // Debug.Log("The dmg to health is: " + (int)(remainDmg));
                    // If shield is depleted, apply remaining damage to health
                    Health -= (int)(remainDmg);

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

    public float dropRateHealth1 = 10f;
    public float dropRateHealth2 = 2f;

    private bool dropRateBoosted = false; // Flag to check if boost is active

    public float boostedDropRateHealth1 = 100f; // Boosted drop rate for health item 1
    public float boostedDropRateHealth2 = 50f; // Boosted drop rate for health item 2

    // Coroutine to temporarily increase the drop rate
    public IEnumerator BoostDropRate(float duration)
    {
        if (!dropRateBoosted)
        {
            dropRateBoosted = true;
            float originalDropRate1 = dropRateHealth1;
            float originalDropRate2 = dropRateHealth2;

            // Apply boosted drop rates
            dropRateHealth1 = boostedDropRateHealth1;
            dropRateHealth2 = boostedDropRateHealth2;

            // Wait for the duration (e.g., 3 seconds)
            yield return new WaitForSeconds(duration);

            // Revert to the original drop rates
            dropRateHealth1 = originalDropRate1;
            dropRateHealth2 = originalDropRate2;

            dropRateBoosted = false;
        }
    }

    private void DropWhenDeath()
    {
        // Generate a random number between 0 and 100
        float randomValue = UnityEngine.Random.Range(0f, 100f);

        // 10% chance to drop item 1
        if (randomValue <= dropRateHealth1 && dropItem1 != null)
        {
            Instantiate(dropItem1, transform.position, Quaternion.identity);
        }
        // 2% chance to drop item 2 (within the remaining 90%)
        else if (randomValue > 0 && randomValue <= dropRateHealth2 && dropItem2 != null)
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
