using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal_2 : Powerups
{
    private float chanceToTrigger = 0.3f;  // 30% xác suất
    private float defenseReduction = 0.1f; // Giảm 10% phòng thủ
    private float effectTimer = 0f;        // Bộ đếm thời gian cho hiệu ứng

    public Metal_2()
    {
        this.id = 2;
        this.ElementId = 1;
        this.cooldown = 5;
        this.duration = 2; // Thời gian hiệu lực của hiệu ứng (2 giây)
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.Debuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.EnemyDefReduction;
    }

    public void OnAttack(Knight target)
    {
        if (Random.value <= chanceToTrigger)
        {
            ApplyDefenseReduction(target);
        }
    }

    private void ApplyDefenseReduction(Knight target)
    {
        if (target != null)
        {
            target.ReduceDefense(defenseReduction);
            effectTimer = this.duration; // Đặt thời gian hiệu ứng (2 giây)
            isActive = true; // Đánh dấu hiệu ứng đang hoạt động
        }
    }

    public void Update(float deltaTime, Knight target)
    {
        if (isActive && effectTimer > 0)
        {
            effectTimer -= deltaTime; // Giảm thời gian theo deltaTime

            if (effectTimer <= 0)
            {
                // Hết thời gian, khôi phục phòng thủ của kẻ địch
                target.RestoreDefense(defenseReduction);
                isActive = false;
            }
        }
    }
}