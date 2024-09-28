using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1", menuName = "Item/Item1")]

public class Item1 : Item
{
    PlayerController playerController;

    private void OnEnable()
    {
        // this.image = "";
        this.itemName = "BerserkFillUp";
        this.itemDescription = "Fill up your Berserk Bar immediately";
        this.itemType = ItemType.Active;
    }

    public override void Activate()
    {
        Debug.Log("restore");
        playerController= FindAnyObjectByType<PlayerController>();
        BerserkGauge bg = playerController.GetComponent<BerserkGauge>();
        bg.IncreaseProgress(bg.maxValue);
    }
}