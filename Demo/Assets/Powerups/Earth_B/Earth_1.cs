using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Earth_1", menuName = "Powerups/Earth_1")]
public class Earth_1 : Powerups
{   
    public float staminaIncrease = 0.15f;
    public Earth_1()
    {
        this.id = 13;
        this.ElementId = 5;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.HpBuff;
    
        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.HPIncrease;
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            player.IncreaseHp(staminaIncrease);
        }
    }
}
