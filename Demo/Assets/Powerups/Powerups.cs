using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups
{
    public int id;
    public BuffType type;  
    public int ElementId;

    public TriggerCondition triggerCondition;
    public Effect effect;

    public enum TriggerCondition
    {
        Always,
        LowHP,
        HitDebuffedEnemy,
        Berserk,
        DmgReduction
    }

    public enum Effect
    {
        DamageIncrease,
        DefenseIncrease,
        HPIncrease,
        SpeedIncrease,
        EnemyDefReduction,
        HPRegen
    }

    public enum BuffType
    {
        DefBuff,       // Buff phòng thủ
        HpBuff,        // Buff máu
        SpeedBuff,     // Buff tốc độ
        DamageBuff,    // Buff sát thương
        Debuff         // Debuff
    }

    public int cooldown;
    public int duration;
    public bool isActive;
    public float currentCooldown;
}
