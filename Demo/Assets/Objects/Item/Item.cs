using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    // Item properties
    public string itemName;
    public string itemDescription;
    public Image image;
    public enum ItemType { Active, Passive }
    public ItemType itemType;
    // public bool autoActivateOnPickup = true;
     public abstract void Activate();

    // // Virtual method to implement item pickup behavior
    // public virtual void OnPickup(Damageable target)
    // {
    //     if (itemType == ItemType.Passive && autoActivateOnPickup)
    //     {
    //         Activate(target);
    //     }
    // }

    // // Virtual method to implement item activation behavior
    // public virtual void Update()
    // {
    //     if (itemType == ItemType.Active && Input.GetKeyDown(activationKey))
    //     {
    //         // Get the target (e.g. the player)
    //         Damageable target = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    //         Activate(target);
    //     }
    // }
}