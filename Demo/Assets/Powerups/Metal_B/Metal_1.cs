using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Metal_1", menuName = "Powerups/Metal_1")]
public class Metal_1 : Powerups
{
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

        this.effect = Effect.EnemyDefReduction;
    }

    public void ApplyEffect(PlayerController player)
    {
        
        Debug.Log("dmm chay di: ");
        if (player != null)
        {
            player.IncreaseDef(0.1f);   
        }
    }
}
