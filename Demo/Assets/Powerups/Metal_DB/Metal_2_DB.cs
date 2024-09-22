using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Metal_2", menuName = "BadEffect/Metal_2")]
public class Metal_2_DB : BadEffect
{
    private float defenseReduction = 0.1f; // Giảm 10% phòng thủ
    private float effectTimer = 0f;        // Bộ đếm thời gian cho hiệu ứng

    public Metal_2_DB()
    {
        this.id = 2;
        this.ElementId = 1;
        this.cooldown = 5;
        this.duration = 1; // Thời gian hiệu lực của hiệu ứng (2 giây)
        this.isActive = false;
        this.currentCooldown = 0f;
        this.type = DebuffType.DefDebuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.EnemyDefReduction;
    }

    public void ApplyDefenseReduction(Knight target)
    {
        if (target != null)
        {
            target.ReduceDefense(duration, defenseReduction);
        }
    }
}