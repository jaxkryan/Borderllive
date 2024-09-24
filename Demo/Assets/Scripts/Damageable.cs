using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    private bool metal3Active = false;
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    public UnityEvent<int, int> healthChanged;
    public GameObject dropItem;
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
            return _health;
        }
        set
        {
            OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0.5 * MaxHealth && !metal3Active && ownedPowerups.IsPowerupActive<Metal_3>())
            {
                //Debug.Log("metal 3 buff is active");
                ownedPowerups.TriggerMetal3Buff();
                metal3Active = true;
            }

            if (_health <= 0.35 * MaxHealth && ownedPowerups.IsPowerupActive<Wood_3>())
            {
                ownedPowerups.TriggerWood3Buff();
            }
            if (_health > 0.5 * MaxHealth && metal3Active)
            {
                ownedPowerups.RemoveMetal3Buff();
                metal3Active = false;
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

            if (_health <= 0)
            {
                IsAlive = false;
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
                    //Debug.Log("Stunned for duration: " + stunDuration + ", stun end time: " + stunEndTime);
                }
            }
        }
    }
    public int XPReward = 500;
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
                GiveXPReward();
                damageableDeath.Invoke();
            }
        }
    }
    private void GiveXPReward()
    {
        XPTracker xpTracker = FindObjectOfType<XPTracker>();
        if (xpTracker != null)
        {
            xpTracker.AddXP(XPReward);
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

        if (_health > 0.5 * MaxHealth && metal3Active)
        {
            OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
            if (ownedPowerups != null)
            {
                ownedPowerups.RemoveMetal3Buff(); // Remove Metal_3 buff
                metal3Active = false; // Reset the flag so the buff can be triggered again if health drops below 50%
            }
        }

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterStat = GetComponent<CharacterStat>();
        if (characterStat == null)
        {
            enemyStat = GetComponent<EnemyStat>();
        }
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
            Health -= (int)reducedDamage;
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
        if (dropItem != null)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }
}
