using UnityEngine;

[CreateAssetMenu(fileName = "Metal_3", menuName = "Powerups/Metal_3")]
public class Metal_3 : Powerups
{
    public Metal_3()
    {
        this.id = 3;
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
        InitializeLocalization("Metal_3_Name", "Metal_3_Description");
    }

    public void ApplyEffect(PlayerController player)
    {
        if (player != null)
        {
            player.IncreaseDefLowHP(0.1f);
        }
    }
}
