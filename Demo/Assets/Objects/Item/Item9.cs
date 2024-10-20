using UnityEngine;

[CreateAssetMenu(fileName = "Item9", menuName = "Item/Item9")]
public class Item9 : Item
{
    private CharacterStat characterStat;
    private Damageable damageable;

    private void OnEnable()
    {
        this.itemName = "Spicy garlic";
        this.itemDescription = "Trade 30% of your current HP for a 100% attack increase for 5 seconds.";
        this.itemType = ItemType.Active;
        this.cd = 20;
        this.cost = 250;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "";
        InitializeLocalization("Item9", "Item9_Description", "Item9_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
        // Find the CharacterStat component on the player
        characterStat = FindObjectOfType<CharacterStat>();
        damageable = characterStat.GetComponent<Damageable>();

        if (characterStat != null && damageable != null)
        {
            // Reduce health by 20%
            float healthToLose = damageable.Health * 0.3f;
            damageable.Hit((int)healthToLose, Vector2.zero);

            // Trigger the effect in CharacterStat
            characterStat.StartSpicyGarlicEffect();
        }
    }
}
