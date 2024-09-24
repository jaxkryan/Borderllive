using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IBuffable
{
    [SerializeField]
    private ScriptableBuff AttackBuff;
    private ScriptableBuff _buff;
    private CharacterStat characterStat; // Reference to CharacterStat
    private float attackDamage;
    public Vector2 knockback = Vector2.zero;

    private void Start()
    {
        // Get the CharacterStat component from the same GameObject
        characterStat = GetComponentInParent<CharacterStat>();
        if (characterStat == null)
        {
            Debug.LogError("EnemierrStat component not found!");
            return;
        }

        ApplyBuff(AttackBuff);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attackDamage = characterStat.Damage;
        Damageable damageable = collision.GetComponent<Damageable>();
        Debug.Log("Trigger entered with: " + collision.gameObject.name);
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit((int)attackDamage, deliveredKnockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDamage);

                if (gotHit)
                {
                    Debug.Log(collision.name + " hit for " + attackDamage);
                    OwnedPowerups ownedPowerups = GetComponentInParent<OwnedPowerups>();
                    Knight enemyKnight = collision.GetComponent<Knight>();
                    if (enemyKnight != null)
                    {
                        ownedPowerups.EnemyHit(); // Set the hit flag
                        ownedPowerups.CheckPowerupEffects(enemyKnight);
                        // Apply the debuff to the hit enemy
                    }
                }
            }
            damageable.IsStun = false;
        }
    }
    public void Start()
    {
        ApplyBuff(AttackBuff);
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