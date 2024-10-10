using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item3", menuName = "Item/Item3")]

public class Item3 : Item
{
    PlayerController playerController;

    private void OnEnable()
    {
        this.itemName = "ShieldForAHealth";
        this.itemDescription = "Trade 5% hp for 10% shield. Can not use skill if under 20% hp.";
        this.itemType = ItemType.Active;
        this.cd = 20f;
        this.cost = 80;
                        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "Trade 5% hp for 10% shield. Can not use skill if under 20% hp.";

        InitializeLocalization("Item3", "Item3_Description", "Item3_HistoryDescription");

    }

    public override void Activate()
    {
         if (isEnable==false) return;
        playerController = FindAnyObjectByType<PlayerController>();
        CharacterStat characterStat = playerController.GetComponent<CharacterStat>();
        Damageable damageable = playerController.GetComponent<Damageable>();
        if (characterStat != null)
            {
                // Check if the player's health is above 50%
                if (damageable.Health > 0.5f * characterStat.MaxHealth)
                {
                    // Calculate the health to deduct (5% of current health)
                    float healthToDeduct = damageable.MaxHealth * 0.05f;
                    // Deduct health
                    damageable.Health -= (int)healthToDeduct;

                    // Increase shield by 10%
                    characterStat.IncreaseShield(characterStat.MaxHealth * 0.1f);

                    // Log the activation
                    Debug.Log($"Activated {itemName}: traded {healthToDeduct} HP for 10% shield.");
                }
                else
                {
                    Debug.LogWarning("Cannot activate DmgAllEnemy: Player health is below 50%.");
                }
            }
            else
            {
                Debug.LogError("CharacterStat component not found on PlayerController.");
            }
        }
}