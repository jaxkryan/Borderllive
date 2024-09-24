using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Metal_2", menuName = "Powerups/Metal_2")]
public class Metal_2 : Powerups
{
    private float chanceToTrigger = 0.1f;  // 10% xác suất
    private float defenseReduction = 0.1f; // Giảm 10% phòng thủ

    public Metal_2()
    {
        this.id = 2;
        this.ElementId = 1;
        this.cooldown = 10;
        this.duration = 1;
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
        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            ApplyDefenseReduction(target);
            return true;
        }
        return false;
    }

    private void ApplyDefenseReduction(Knight target)
    {
        if (target != null)
        {
            target.ReduceDefense(this.id, this.duration, defenseReduction);
        }
    }
}