using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using static Item;

public abstract class Item : ScriptableObject
{
    // Item properties
    public string itemName;
    public string itemDescription;
    public string historyDescription;
    public Sprite image;
    public enum ItemType { Active, Passive }
    public ItemType itemType;
    public float cd;
    public int cost;
    
    public int unlockCost;
    public String code;
    public bool isEnable;
    // public bool autoActivateOnPickup = true;
     public abstract void Activate();
    public string GetImageName()
    {
        return image != null ? image.name : null; // Get the sprite name
    }

    public LocalizedString nameLocalization;
    public LocalizedString descriptionLocalization;
    public LocalizedString historyDescriptionLocalization; // Localization for historyDescription

    public void InitializeLocalization(string nameKey, string descriptionKey, string historyKey)
    {
        nameLocalization = new LocalizedString { TableReference = "Items", TableEntryReference = nameKey };
        descriptionLocalization = new LocalizedString { TableReference = "Items", TableEntryReference = descriptionKey };
        historyDescriptionLocalization = new LocalizedString { TableReference = "Items", TableEntryReference = historyKey }; // Initialize localization for history description
    }

    public void UpdateHistoryLocalization(string historyKey)
    {
        historyDescriptionLocalization.TableEntryReference = historyKey;  // Method to update the history description localization
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
    private const string PlayerPrefsKeyPrefix = "ItemState_";
     public void SaveItemState()
    {
        string key = PlayerPrefsKeyPrefix + itemName;
        PlayerPrefs.SetInt(key, isEnable ? 1 : 0);
        // Debug.Log($"PlayerPrefs value for {key}: {PlayerPrefs.GetInt(key)}");

    }

    // Method to load the state of this item from PlayerPrefs
    public void LoadItemState()
    {
        if (isEnable) return;
        string key = PlayerPrefsKeyPrefix + itemName;
        // Debug.Log($"PlayerPrefs value for {key}: {PlayerPrefs.GetInt(key)}");
        if (PlayerPrefs.HasKey(key))
        {
            isEnable = PlayerPrefs.GetInt(key) == 1;
        }
        else
        {
            isEnable = false; // Default state if no data is saved
        }
    }
}