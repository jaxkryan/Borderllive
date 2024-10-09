using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item1", menuName = "Item/Item1")]

public class Item1 : Item
{
    PlayerController playerController;

    private void OnEnable()
    {
        this.itemName = "BerserkFillUp";
        this.itemDescription = "Fill up your Berserk Bar immediately";
        this.itemType = ItemType.Active;
        this.cd = 30f;
        this.cost = 100;
        this.code = "daikalop12a";
        this.isEnable = true;
    }

    public override void Activate()
    {
        if (isEnable==false) return;
        playerController= FindAnyObjectByType<PlayerController>();
        
        BerserkGauge bg = playerController.GetComponent<BerserkGauge>();
        bg.IncreaseProgress(bg.maxValue);
    }
}