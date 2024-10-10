using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item6", menuName = "Item/Item6")]

public class Item6 : Item
{
    public float healAmount = 0.2f;

    private void OnEnable()
    {
        this.itemName = "HealDynamic";
        this.itemDescription = "The lower your health is, the higher hp you recorver";
        this.itemType = ItemType.Active;
        this.cd = 0;
        this.cost = 100;
        this.code = "daikalop12a";
        this.isEnable = true;
        this.historyDescription = "";
        InitializeLocalization("Item6", "Item6_Description", "Item6_HistoryDescription");
    }

    public override void Activate()
    {
        return;
    }
}
