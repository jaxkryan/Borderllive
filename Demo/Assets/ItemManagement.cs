using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagement : MonoBehaviour
{
    private List<Item> activeItems = new List<Item>();  // Items available in the shop
    public GameObject item;  // Prefab to display each item in the UI
    public Transform image;   // Parent object to hold the item buttons in the UI
    public OwnedActiveItem ownedActiveItem;  // Reference to OwnedActiveItem script

    private PlayerController playerController;

        private void Start()
    {
        DisplayExchangeItems();
        playerController = FindObjectOfType<PlayerController>();
        ownedActiveItem = playerController.GetComponent<OwnedActiveItem>();
    }
    // private void Update(){
    //     DisplayExchangeItems();
    // }
    public void DisplayExchangeItems()
    {
        if (ownedActiveItem.item1!=null) {
            activeItems.Add(ownedActiveItem.item1);
        }

        if (ownedActiveItem.item2!=null) {
            activeItems.Add(ownedActiveItem.item2);
        }
        if (activeItems.Count == 0) return; 
        foreach (Item activeItems in activeItems)
        {
            GameObject newShopPanel = Instantiate(item, image);
            ItemInventoryUI itemInventoryUI = newShopPanel.GetComponent<ItemInventoryUI>();

            if (itemInventoryUI != null)
            {
                itemInventoryUI.SetItem(activeItems, this);
                newShopPanel.SetActive(true);
                // Debug.Log("Item added to shop: " + item.itemName);
            }
            else
            {
                Debug.LogError("ShopItemUI component missing on shop panel prefab!");
            }
        }
    }

    // Method to handle exchanging items
    public void ExchangeItem(Item newItem, ItemInventoryUI newItemUI)
    {
        // Ensure ownedActiveItem has 2 items (item1 and item2)
        if (ownedActiveItem.item1 != null && ownedActiveItem.item2 != null)
        {
            // Show a UI to choose which item to replace (This can be a new UI panel that appears)
            ShowExchangeOptions(newItem);
        }
        else
        {
            // If there's only 1 item, directly add the new item to the second slot
            if (ownedActiveItem.item1 == null)
            {
                ownedActiveItem.item1 = newItem;
            }
            else
            {
                ownedActiveItem.item2 = newItem;
            }
            // DisplayActiveItems(); // Update the UI to reflect the new items
        }
    }

    // Show a UI to select which item to replace
    private void ShowExchangeOptions(Item newItem)
    {
        // Assuming you have a UI set up where the player can choose which item to exchange.
        // This could be a simple popup dialog with two buttons, one for each existing item.

        // Example of how this can be handled:
        Debug.Log("Choose which item to exchange for the new item: " + newItem.itemName);

        // Use UI buttons for each active item (item1 and item2) to allow the player to choose
        // For this, you'll need to set up a UI that allows the player to select the item to replace
        // Here's an example of the logic:
        
        // Let's assume you set up buttons for item1 and item2 in your UI:
        // Button 1:
        ExchangeButton(ownedActiveItem.item1, newItem);

        // Button 2:
        ExchangeButton(ownedActiveItem.item2, newItem);
    }

    // Method to handle button click for exchanging items
    private void ExchangeButton(Item oldItem, Item newItem)
    {
        // Replace the old item with the new one
        if (ownedActiveItem.item1 == oldItem)
        {
            ownedActiveItem.item1 = newItem;
        }
        else if (ownedActiveItem.item2 == oldItem)
        {
            ownedActiveItem.item2 = newItem;
        }

        // Refresh the UI to reflect the new item
        // DisplayActiveItems();
    }
}
