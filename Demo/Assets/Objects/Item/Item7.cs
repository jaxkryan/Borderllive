using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item7", menuName = "Item/Item7")]
public class Item7 : Item
{
    private void OnEnable()
    {
        this.itemName = "Revive";
        this.itemDescription = "When your HP is 0, you will be revived with 50% HP. Automatically trigger and only used once per item. You can only carry one of this item.";
        this.itemType = ItemType.Passive;
        this.cd = 0;
        this.cost = 300;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "";
        InitializeLocalization("Item7", "Item7_Description", "Item7_HistoryDescription");
    }

    public override void Activate()
    {
        // Get the player controller or any reference to the player
        GameObject playerController = GameObject.FindWithTag("Player"); // Assuming the player has the tag "Player"
        
        // Ensure the player is found
        if (playerController == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        // Get the Damageable component attached to the player
        Damageable damageable = playerController.GetComponent<Damageable>();

        // Ensure that the Damageable component is found
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on the player!");
            return;
        }

        // Log the player's current status
        Debug.Log("Is player alive? " + damageable.IsAlive);

        // Check if the player is dead (Health <= 0 and IsAlive == false)
        if (damageable.IsAlive == true)
        {
            // Revive the player with 50% of MaxHealth
            damageable.Health = damageable.MaxHealth / 2;
            damageable.IsAlive = true;
            Debug.Log($"{itemName} has been used to revive the player. Player health is now {damageable.Health}.");

            // Remove this item after use
            var ownedActiveItem = playerController.GetComponent<OwnedActiveItem>();
            if (ownedActiveItem != null)
            {
                 Debug.Log("Removing item after revive...");
                ownedActiveItem.RemoveItem(this); // Remove Item7 from inventory
            }
        }
        else
        {
            Debug.Log("Player is still alive. No need to use the revive.");
        }
    }
}
