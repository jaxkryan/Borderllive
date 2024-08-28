using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
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
            _health = value;

            healthChanged?.Invoke(_health, MaxHealth);
            if(_health <= 0)
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
                damageableDeath.Invoke();
            }
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            //  Debug.Log("Hit! Health: " + Health);
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
