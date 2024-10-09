using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public abstract class Item : ScriptableObject
{
    // Item properties
    public string itemName;
    public string itemDescription;
    public Sprite image;
    public enum ItemType { Active, Passive }
    public ItemType itemType;
    public float cd;
    public int cost;
    public String code;
    public bool isEnable = true;
    // public bool autoActivateOnPickup = true;
     public abstract void Activate();
    public string GetImageName()
    {
        return image != null ? image.name : null; // Get the sprite name
    }
    [field: SerializeField]
    public List<ItemParameter> DefaultParametersList { get; set; }
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

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}