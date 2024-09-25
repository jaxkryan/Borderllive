using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerups;

[CreateAssetMenu(fileName = "Water_3", menuName = "Powerups/Water_3")]
public class Water_3 : Powerups
{
    private float reducePercent = 0.1f;
    public Water_3()
    {
        this.id = 8;
        this.ElementId = 3;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.BerserkBuff;

        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.BeserkRegen;
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            player.ReduceBerserkPenalty(reducePercent);
        }
    }
}
