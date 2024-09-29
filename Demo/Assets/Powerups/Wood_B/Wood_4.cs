using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood_4", menuName = "Powerups/Wood_4")]

public class Wood_4 : Powerups
{
    public float healingPercentage = 0.01f;
    public Wood_4()
    {
        this.id = 12;
        this.ElementId = 2;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.HpBuff;
        this.triggerCondition = TriggerCondition.Berserk;
        this.effect = Effect.HPRegen;
    }


}
