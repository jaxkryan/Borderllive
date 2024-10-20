using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagement : MonoBehaviour
{
    private List<Item> activeItems = new List<Item>();  // Items available in the shop
    public GameObject item;  // Prefab to display each item in the UI
    public Transform image;   // Parent object to hold the item buttons in the UI
    public OwnedActiveItem ownedActiveItem;  // Reference to OwnedActiveItem script
    public GameObject exchangeScreen;
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        ownedActiveItem = playerController.GetComponent<OwnedActiveItem>();
        DisplayExchangeItems();
    }

    public void DisplayExchangeItems()
    {
        // Clear previous activeItems to avoid duplicates
        activeItems.Clear();

        // Add item1 and item2 to the list
        if (ownedActiveItem.item1 != null) {
            activeItems.Add(ownedActiveItem.item1);
        }

        if (ownedActiveItem.item2 != null) {
            activeItems.Add(ownedActiveItem.item2);
        }

        // // Check for the 3rd item (current)
        // if (ownedActiveItem.currentItem != null) {
        //     activeItems.Add(ownedActiveItem.currentItem);
        // }

        // Ensure we have items to display
        if (activeItems.Count == 0) return;

        // Display each active item
        foreach (Item item in activeItems)
        {
            GameObject newShopPanel = Instantiate(this.item, image);
            ItemInventoryUI itemInventoryUI = newShopPanel.GetComponent<ItemInventoryUI>();

            if (itemInventoryUI != null)
            {
                itemInventoryUI.SetItem(item, this);
                newShopPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("ShopItemUI component missing on shop panel prefab!");
            }
        }
    }

    // Method to handle exchange logic
    public void ExchangeItem(Item changeItem)
    {
        // You can add UI to let the player choose between item1 and item2 for the exchange
        Debug.Log("Exchanging item " + changeItem.name);

        // Example: automatically exchange with item1 (you can enhance this part to let the player choose)
        if (ownedActiveItem.item1 != changeItem)
        {
            Debug.Log("Item 1 exchanged with new item.");
            ownedActiveItem.item2 = ownedActiveItem.currentItem;
        }
        else 
        {
            ownedActiveItem.item1 = ownedActiveItem.currentItem;
        }
        ownedActiveItem.UpdateUI();
        exchangeScreen.SetActive(false);
        // Update the UI to reflect the exchange
        
       // DisplayExchangeItems();
    }
}

