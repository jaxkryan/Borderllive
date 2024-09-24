using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood_2", menuName = "Powerups/Wood_2")]
public class Wood_2 : Powerups
{
    private int hitCount = 0;
    private float defenseReduction = 0.1f;

    public Wood_2()
    {
        this.id = 2;
        this.ElementId = 1;
        this.cooldown = 10;
        this.duration = 2;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.Debuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.EnemyDefReduction;
    }

    public Boolean ApplyEffect(Knight target)
    {
            ApplyDefenseReduction(target);
            hitCount = 0;
            return true;
    }

    private void ApplyDefenseReduction(Knight target)
    {
        if (target != null)
        {
            target.ReduceDefense(this.id, this.duration, defenseReduction);
        }
    }
}

