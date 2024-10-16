using UnityEngine;

[CreateAssetMenu(fileName = "Item10", menuName = "Item/Item10")]
public class Item10 : Item
{
    private CharacterStat characterStat;
    private Damageable damageable;

    private void OnEnable()
    {
        this.itemName = "Delicious leaf";
        this.itemDescription = "Use half of your shield value to heal yourself. If there is no shield, give 10 shield points.";
        this.itemType = ItemType.Active;
        this.cd = 30;
        this.cost = 250;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "";
        InitializeLocalization("Item10", "Item10_Description", "Item10_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
        // Ensure the CharacterStat and Damageable components are set up
        if (characterStat == null)
        {
            characterStat = FindObjectOfType<CharacterStat>();  // Or however you access it
        }
        if (damageable == null && characterStat != null)
        {
            damageable = characterStat.GetComponent<Damageable>();
        }
        // Check if the character has shield points
        if (characterStat.Shield > 0 && damageable.Health < damageable.MaxHealth)
        {
            // Consume half the shield and heal the player by that amount
            float shieldToConsume = characterStat.Shield / 2f;
            characterStat.DecreaseShield(shieldToConsume);
            damageable.Heal((int)shieldToConsume);  // Assuming Heal() method exists on the Damageable component
        }
        else
        {
            // Debug.Log("health: " + damageable.Health);
            // Debug.Log("max health: " + damageable.MaxHealth);
            // Grant 10 shield points if no shield exists
            characterStat.IncreaseShield(10f);
        }

        // Apply cooldown and other item logic
        this.isEnable = false;  // Disable the item after activation
        SaveItemState();  // Save state if necessary
    }

}
