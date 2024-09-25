using UnityEngine;

[CreateAssetMenu(fileName = "Water_1", menuName = "BadEffect/Water_1")]
public class Water_1_DB : BadEffect
{
    private float defenseReduction = 0.1f; // Giảm 10% phòng thủ
    private float effectTimer = 0f;        // Bộ đếm thời gian cho hiệu ứng

    public Water_1_DB()
    {
        this.id = 6;
        this.ElementId = 2;
        this.cooldown = 10;
        this.duration = 10;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.type = DebuffType.DefDebuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.SpeedDecrease;
    }


    //public void ApplyDefenseReduction(Knight target)
    //{
    //    if (target != null)
    //    {
    //        target.ReduceDefense(duration, defenseReduction);
    //    }
    //}
}