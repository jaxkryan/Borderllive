using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerups;

[CreateAssetMenu(fileName = "Water_2", menuName = "Powerups/Water_2")]
public class Water_2 : Powerups
{
    public Water_2()
    {
        this.id = 7;
        this.ElementId = 3;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.SpeedBuff;

        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.SpeedIncrease;
        InitializeLocalization("Water_2_Name", "Water_2_Description");
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            player.IncreaseAgility();
        }
    }
}
