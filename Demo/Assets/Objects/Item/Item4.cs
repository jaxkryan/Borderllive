using UnityEngine;

[CreateAssetMenu(fileName = "Item4", menuName = "Item/Item4")]
public class Item4 : Item
{
    private float boostedSoulMultiplier = 1.5f;
    private float duration = 10f;
    private CurrencyManager currencyManager; // Reference to the CurrencyManager

    public override void Activate()
    {
        // Find the CurrencyManager in the scene (or pass it in)
        if (currencyManager == null)
        {
            currencyManager = FindObjectOfType<CurrencyManager>();
        }

        // Trigger the soul absorption boost through the CurrencyManager
        if (currencyManager != null)
        {
            currencyManager.BoostSoulDrop(boostedSoulMultiplier, duration);
        }
        else
        {
            Debug.LogError("CurrencyManager not found in the scene!");
        }
    }

    private void OnEnable()
    {
        this.itemName = "SoulCenser";
        this.itemDescription = "For the next 10 seconds, increase your soul absorption by 150%.";
        this.itemType = ItemType.Active;
        this.cd = 30f;
        this.cost = 120;
        this.code = "daikalop12a";
        this.isEnable = true;
        this.historyDescription = "At the end of the 16th century, Vietnamese people often placed Chu Dau pottery kilns as incense burners to offer in religious works. The engraving of the two words \"Dai Tu\" is the word Buddha, proving that the role of Buddhism in the Vietnamese mind is very important. After \"thousands of years of Northern colonization\", Vietnamese people still pose incense burners with the shape of Dong Son drums.";
        InitializeLocalization("Item4", "Item4_Description", "Item4_HistoryDescription");

    }
}
