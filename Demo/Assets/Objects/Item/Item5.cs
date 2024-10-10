using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item5", menuName = "Item/Item5")]

public class Item5 : Item
{
    public float healAmount = 0.2f;

    private void OnEnable()
    {
        this.itemName = "HealStatic";
        this.itemDescription = "Heal 20% of your max HP";
        this.itemType = ItemType.Active;
        this.cd = 0;
        this.cost = 100;
        this.code = "daikalop12a";
        this.isEnable = true;
        this.historyDescription = "";
    }

    public override void Activate()
    {
        return;
    }
}
