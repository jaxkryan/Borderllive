using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood_3", menuName = "Powerups/Wood_3")]
public class Wood_3 : Powerups
{
    public Wood_3()
    {
        this.id = 5;
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

        InitializeLocalization("Wood_3_Name", "Wood_3_Description");
    }

    internal void ApplyEffect(PlayerController player)
    {
            if (player != null)
            {
                player.HPRegen();
           }
    }

}
