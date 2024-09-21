using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        this.type = BuffType.DefBuff;

        this.triggerCondition = TriggerCondition.Always;

        this.effect = Effect.EnemyDefReduction;
    }
}
