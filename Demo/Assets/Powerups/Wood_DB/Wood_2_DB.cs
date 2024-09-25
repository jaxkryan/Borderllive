using UnityEngine;

[CreateAssetMenu(fileName = "Wood_2", menuName = "BadEffect/Wood_2")]
public class Wood_2_DB : BadEffect
{
    private float defenseReduction = 0.1f; // Giảm 10% phòng thủ
    private float effectTimer = 0f;        // Bộ đếm thời gian cho hiệu ứng

    public Wood_2_DB()
    {
        //no cd but count by hitcount. But i still add it for control purpose
        this.id = 4;
        this.ElementId = 2;
        this.cooldown = 2;
        this.duration = 2;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.type = DebuffType.DefDebuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.EnemyDefReduction;
    }


    //public void ApplyDefenseReduction(Knight target)
    //{
    //    if (target != null)
    //    {
    //        target.ReduceDefense(duration, defenseReduction);
    //    }
    //}
}