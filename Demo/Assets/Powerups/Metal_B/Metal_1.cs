﻿using UnityEngine;

[CreateAssetMenu(fileName = "Metal_1", menuName = "Powerups/Metal_1")]
public class Metal_1 : Powerups
{
    public float enduranceIncrease = 0.1f;
    public Metal_1()
    {
        this.id = 1;
        this.ElementId = 1;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.DefBuff;

        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.DefenseIncrease;
        InitializeLocalization("Metal_1_Name", "Metal_1_Description");
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            player.IncreaseDef(enduranceIncrease);
        }
    }
}
