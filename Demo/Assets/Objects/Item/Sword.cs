using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Item/Sword")]
public class Sword : Item
{


    private void OnEnable()
    {
        this.itemName = "sword";
        this.itemDescription = "This is a sword. You don't understand why every time you place it on the ground, it splits into three parts: the hilt, the blade, and the guard, then starts jumping around you.";
        this.itemType = ItemType.Active;
        this.cd = 0;
        this.cost = 1000;
        this.code = "daikalop12a";
        this.isEnable = false;
        this.historyDescription = "\"Thuan Thien\" means to follow the will of Heaven. Its meaning affirms that Le Loi’s uprising against the Ming Dynasty was in accordance with Heaven’s will, ensuring inevitable victory. The Thuan Thien sword is said to possess a mystical power, which granted Le Loi immense strength, making him appear larger and endowed with the power of ten thousand men.";
        InitializeLocalization("Sword", "Sword_Description", "Sword_HistoryDescription");
        LoadItemState();
    }

    public override void Activate()
    {
        return;
    }
}

