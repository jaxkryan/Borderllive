using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Earth_2", menuName = "Powerups/Earth_2")]
public class Earth_2 : Powerups
{
   public Earth_2()
    {
        this.id = 14;
        this.ElementId = 5;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.DefBuff;

        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.DefenseIncrease;
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            
            player.IncreaseDefHighHp(2f);
        }
        else
        {
            Debug.Log("No player found");
        }
    }
}
