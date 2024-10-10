using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour, IBuffable
{
    [SerializeField]
    private ScriptableBuff AttackBuff;
    private ScriptableBuff _buff;
    private CharacterStat characterStat; // Reference to CharacterStat
    private EnemyStat enemyStat; // Reference to EnemyStat
    public float attackDamage;
    public Vector2 knockback = Vector2.zero;
    private OwnedPowerups ownedPowerups;

    // Reference to BerserkBar script
    private BerserkGauge berserkBar;

    //private OwnedPowerups ownedPowerups;
    private void Start()
    {
        // Check if this script is attached to the player by checking the tag
        if (transform.root.CompareTag("Player"))
        {

            ownedPowerups = GetComponentInParent<OwnedPowerups>();
            // Get the CharacterStat component from the parent object
            characterStat = GetComponentInParent<CharacterStat>();
            if (characterStat == null)
            {
                Debug.LogError("CharacterStat component not found on the player!");
                return;
            }

            // Find and reference the BerserkBar component
            berserkBar = FindObjectOfType<BerserkGauge>();
            if (berserkBar == null)
            {
                Debug.LogError("BerserkBar component not found in the scene!");
            }

            ApplyBuff(AttackBuff);
        }
        else
        {
            // Handle for enemies or disable if not needed
            enemyStat = GetComponentInParent<EnemyStat>();
            if (enemyStat == null)
            {
                Debug.LogError("EnemyStat component not found on the enemy!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySFX("PlayerAttack");

        // Determine attack damage based on whether it's a player or enemy attack
        if (characterStat != null)
        {
            attackDamage = characterStat.Damage;
            if (ownedPowerups.IsPowerupActive<Fire_1>())
            {
                Fire_1 f1 = new Fire_1();
                Damageable targetDamageable = collision.GetComponent<Damageable>();

                // Apply the Fire_1 buff's damage increase
                attackDamage *= f1.CalculateDamageIncrease(targetDamageable);
            }
            if (ownedPowerups.IsPowerupActive<Fire_2>())
            {
                Fire_2 f2 = new Fire_2();
                Damageable targetDamageable = collision.GetComponent<Damageable>();
                attackDamage *= f2.CalculateDamageIncrease(targetDamageable);
            }
        }
        else
        {
            attackDamage = enemyStat.Damage;
        }
        Damageable damageable = collision.GetComponent<Damageable>();
        //Debug.Log("Trigger entered with: " + collision.gameObject.name);
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit((int)attackDamage, deliveredKnockback);

            if (gotHit)
            {
                OwnedPowerups ownedPowerups = GetComponentInParent<OwnedPowerups>();
                //OwnedPowerups ownedPowerups = GetComponentInParent<OwnedPowerups>();
                //Debug.Log(collision.name + " hit for " + attackDamage);

                // Increase the Berserk Bar only if the attack is from the player
                if (characterStat != null && berserkBar != null)
                {
                    berserkBar.IncreaseProgress(8.6f); // Increase the bar by 8.6 points, adjust value as needed
                }

                if (characterStat != null && berserkBar._isBerserkActive && ownedPowerups.IsPowerupActive<Wood_4>())
                {
                    Debug.Log("ok");    
                    Wood_4 buff = new Wood_4();
                    //// Get the Damageable component of the player
                    Damageable playerDamageable = GetComponentInParent<Damageable>();

                    //// Calculate the amount of HP to heal (1% of max HP)
                    int healAmount = Mathf.RoundToInt(buff.healingPercentage * playerDamageable.MaxHealth);

                    //// Heal the player
                    playerDamageable.Heal(healAmount);
                }
                Knight enemyKnight = collision.GetComponent<Knight>();
                //PlayerController playerController = collision.GetComponent<PlayerController>();

                if (enemyKnight != null)
                {
                    //Debug.Log(collision.name + " hit for " + attackDamage);

                    ownedPowerups.EnemyHit(); // Set the hit flag
                    //Debug.Log("is hit?" + ownedPowerups.isHitEnemy);
                    //Debug.Log("hit count: " + ownedPowerups.hitCount);
                    ownedPowerups.CheckPowerupEffects(enemyKnight);
                    // Apply the debuff to the hit enemy
                }
            }
            damageable.IsStun = false;
        }
    }
    private void Update()
    {
        if (_buff != null) HandleBuff();
    }

    public void ApplyBuff(ScriptableBuff buff)
    {
        this._buff = buff;
        attackDamage += _buff.Value;
    }

    public void RemoveBuff()
    {
        attackDamage -= _buff.Value;
        _buff = null;
    }

    private float currentEffectTime = 0f;

    public void HandleBuff()
    {
        currentEffectTime += Time.deltaTime;

        if (currentEffectTime >= _buff.Duration) RemoveBuff();
    }
}
