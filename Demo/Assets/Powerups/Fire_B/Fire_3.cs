using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire_3", menuName = "Powerups/Fire_3")]
public class Fire_3 : Powerups
{
    //private float chanceToTrigger = 0.05f;

    //idk why when increasePercent pass to IncreaseBerserkRecharge, its always 0
    private float increasePercent = 0.1f;
    public Fire_3()
    {
        this.id = 11;
        this.ElementId = 4;
        this.cooldown = 0;
        this.duration = 0;
        this.isActive = false;
        this.currentCooldown = 0f;
        this.Weight = 10;
        this.BerserkRateIncrease = 0;
        this.type = BuffType.BerserkBuff;
        this.triggerCondition = TriggerCondition.Always;
        this.effect = Effect.BeserkRegen;
        InitializeLocalization("Fire_3_Name", "Fire_3_Description");
    }


    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            //Debug.Log("inc pc: " + increasePercent);
            player.IncreaseBerserkRecharge(increasePercent);
        }
    }
}
