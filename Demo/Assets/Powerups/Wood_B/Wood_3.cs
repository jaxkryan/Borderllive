using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood_3", menuName = "Powerups/Wood_3")]
public class Wood_3 : Powerups
{
    public Wood_3()
    {
        this.id = 1;
        this.ElementId = 2;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.HpBuff;

        this.triggerCondition = TriggerCondition.LowHP;

        this.effect = Effect.HPRegen;
    }

    internal void ApplyEffect(PlayerController player)
    {
            if (player != null)
            {
                player.HPRegen();
           }
    }

}
