using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerups;

[CreateAssetMenu(fileName = "Water_1", menuName = "Powerups/Water_1")]
public class Water_1 : Powerups
{
    private float chanceToTrigger = 0.1f;  
    private float speedReduction = 0.5f;

    public Water_1(){
        this.id = 6;
        this.ElementId = 3;
        this.cooldown = 7;
        this.duration = 3;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.Debuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.SpeedDecrease;
        InitializeLocalization("Water_1_Name", "Water_1_Description");
    }

    public Boolean ApplyEffect(Knight target)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= chanceToTrigger)
        {
            ApplySlow(target);
            return true;
        }
        return false;
    }

    private void ApplySlow(Knight target)
    {
        if (target != null)
        {
            Debug.Log("water 1 in duration: " + this.duration);
            target.SlowDown(id, this.duration, speedReduction);
        }
    }
}
