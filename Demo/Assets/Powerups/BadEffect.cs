using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEffect : ScriptableObject
{
    public int id;
    public DebuffType type;
    public int ElementId;
    public TriggerCondition triggerCondition;
    public Effect effect;
    public int cooldown;
    public int duration;
    public bool isActive;
    public float currentCooldown;

    public enum TriggerCondition
    {
        Always,
        LowHP,
        BeingHitWhileDebuff,
        DmgReduction
    }

    public enum Effect
    {
        DamageDecrease,
        DefenseDecrease,
        HPDecrease,
        SpeedDecrease,
        EnemyDefReduction
    }

    public enum DebuffType
    {
        DefDebuff,
        HpDebuff,
        SpeedDebuff,
        DamageDebuff
    }
}