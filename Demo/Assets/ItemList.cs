using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public GameObject itemUIPrefab;   // Prefab to represent each item in the UI
    public Transform inventoryContent; // Parent object where the item UI elements will be added
    public List<Item> ownedItems;     // List of all items owned by the player

    public PlayerController playerController;
    private OwnedActiveItem ownedActiveItem;    
    private void Start()
    {   

        if (playerController != null)
        {
            ownedActiveItem = playerController.GetComponent<OwnedActiveItem>();
        }
    
        if (ownedActiveItem != null)
        {
            ownedItems.Add(ownedActiveItem.item1);
            ownedItems.Add(ownedActiveItem.item2);
        }

        // // Initially display the inventory
        // UpdateInventoryUI();
    }

    // private void Update(){
    //     if (ownedItems.Count>2) return;
    //     if (ownedActiveItem.item1 != null) ownedItems.Add(ownedActiveItem.item1);
    //     if (ownedActiveItem.item2 != null) ownedItems.Add(ownedActiveItem.item2);
    //     foreach (Item item in ownedItems)
    //     {
    //         GameObject itemUI = Instantiate(itemUIPrefab, inventoryContent);
    //     }
    // }
    // Method to add an item to the player's inventory
    // public void AddToInventory(Item newItem)
    // {
    //     ownedItems.Add(newItem); // Add the item to the list
    //     UpdateInventoryUI(); // Refresh the UI to show the new item
    // }

    // Method to update the inventory UI
    // public void UpdateInventoryUI()
    // {
    //     // Clear previous items from the UI
    //     foreach (Transform child in inventoryContent)
    //     {
    //         Destroy(child.gameObject);
    //     }

    //     // Loop through owned items and create a UI element for each one
    //     foreach (Item item in ownedItems)
    //     {
    //         GameObject itemUI = Instantiate(itemUIPrefab, inventoryContent);
    //     }
    // }
}
