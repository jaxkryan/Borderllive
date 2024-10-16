using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item9", menuName = "Item/Item9")]
public class Item9 : Item
{
    private CharacterStat characterStat;

    private void OnEnable()
    {
        this.itemName = "Equilibrium Scale";
        this.itemDescription = "In 3 seconds, lose 100% Attack and gain 300% Defense.";
        this.itemType = ItemType.Passive;
        this.cd = 10;
        this.cost = 150;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "";
        InitializeLocalization("Item8", "Item8_Description", "Item8_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
        characterStat = FindObjectOfType<CharacterStat>();  // Find the CharacterStat component in the scene
        if (characterStat != null)
        {
            // Call the method to start the coroutine in CharacterStat
            characterStat.StartEquilibriumEffectCoroutine(3f, 0f, 6f);  // 0% Strength, 600% Endurance = 300%def for 3 seconds
        }
    }

}
