using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire_2", menuName = "Powerups/Fire_2")]
public class Fire_2 : Powerups
{
    private float chanceToTrigger = 0.05f;
    public Fire_2()
    {
        this.id = 10;
        this.ElementId = 4;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.DamageBuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.DamageIncrease;
        InitializeLocalization("Fire_2_Name", "Fire_2_Description");
    }

    public float CalculateDamageIncrease(Damageable target)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            return 2f; // 200% damage increase
        }
        else
        {
            return 1f; // No damage increase
        }
    }
}
