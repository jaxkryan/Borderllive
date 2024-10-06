using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class Powerups : ScriptableObject
{
    public int id;
    public string name;
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
        DmgReduction,
        EnemyLowHP
    }

    public enum Effect
    {
        DamageIncrease,
        DefenseIncrease,
        HPIncrease,
        SpeedIncrease,
        SpeedDecrease,
        EnemyDefReduction,
        HPRegen,
        BeserkRegen,
        PushBack,
        Shield
    }

    public enum BuffType
    {
        DefBuff,       // Buff phòng thủ
        BerserkBuff,
        HpBuff,        // Buff máu
        SpeedBuff,     // Buff tốc độ
        DamageBuff,    // Buff sát thương
        Debuff,         // Debuff
        OtherBuff
    }
    public int BerserkRateIncrease;
    public int cooldown;
    public int duration;
    public bool isActive;
    public float currentCooldown = 0f;

    public string description;

    public LocalizedString nameLocalization;
    public LocalizedString descriptionLocalization;

    public void InitializeLocalization(string nameKey, string descriptionKey)
    {
        nameLocalization = new LocalizedString { TableReference = "Powerups", TableEntryReference = nameKey };
        descriptionLocalization = new LocalizedString { TableReference = "Powerups", TableEntryReference = descriptionKey };
    }
    public void UpdateNameLocalization(string nameKey)
    {
        nameLocalization.TableEntryReference = nameKey;
    }

    public void UpdateDescriptionLocalization(string descriptionKey)
    {
        descriptionLocalization.TableEntryReference = descriptionKey;
    }
}
