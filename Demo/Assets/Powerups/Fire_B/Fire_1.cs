using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire_1", menuName = "Powerups/Fire_1")]
public class Fire_1 : Powerups
{
    private float dmgIncrease = 0.15f;
    private float hpThreshold = 0.5f;

    public Fire_1()
    {
        this.id = 9;
        this.ElementId = 4;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.DamageBuff;

        this.triggerCondition = TriggerCondition.EnemyLowHP;

        this.effect = Effect.DamageIncrease;
    }

    public float CalculateDamageIncrease(Damageable target)
    {
        if (target.Health <= 0.5f * target.MaxHealth)
        {
            return 1.15f; // 15% damage increase
        }
        else
        {
            return 1f; // No damage increase
        }
    }
}