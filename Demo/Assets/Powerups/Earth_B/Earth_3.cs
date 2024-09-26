using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Earth_3", menuName = "Powerups/Earth_3")]
public class Earth_3 : Powerups
{
    private float shieldValue = 0.2f;
   public Earth_3()
    {
        this.id = 15;
        this.ElementId = 5;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.OtherBuff;

        this.triggerCondition = TriggerCondition.LowHP;

        this.effect = Effect.Shield;
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player!=null){
            player.IncreaseShield(shieldValue);
        }
    }
}
