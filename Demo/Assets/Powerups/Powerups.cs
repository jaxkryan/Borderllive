using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : ScriptableObject
{
    public int id;
    public BuffType type;  
    public int ElementId;

    public TriggerCondition triggerCondition;
    public Effect effect;

    public int Weight;
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
        HPRegen,
        BeserkRegen
    }

    public enum BuffType
    {
        DefBuff,       // Buff phòng thủ
        BerserkBuff,
        HpBuff,        // Buff máu
        SpeedBuff,     // Buff tốc độ
        DamageBuff,    // Buff sát thương
        Debuff         // Debuff
    }
    public int BerserkRateIncrease;
    public int cooldown;
    public int duration;
    public bool isActive;
    public float currentCooldown = 0f;
}
