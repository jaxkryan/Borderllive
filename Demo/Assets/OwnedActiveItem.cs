using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedActiveItem : MonoBehaviour
{
    // The two active items that are currently owned
    public Item item1;
    public Item item2;

    // The current item that is being used (either item1 or item2)
    public Item currentItem;

    // The key to select item 1
    public KeyCode selectItem1Key = KeyCode.Alpha1;

    // The key to select item 2
    public KeyCode selectItem2Key = KeyCode.Alpha2;

    // The key to confirm the selection
    public KeyCode confirmKey = KeyCode.Return;

    private bool isSelectingItem = false;
    private int selectedItemIndex = 0;

    private void Update()
    {
        // Check if the player is selecting an item
        if (isSelectingItem)
        {
            // Check if the player pressed 1 or 2 to select an item
            if (Input.GetKeyDown(selectItem1Key))
            {
                selectedItemIndex = 0;
            }
            else if (Input.GetKeyDown(selectItem2Key))
            {
                selectedItemIndex = 1;
            }

            // Check if the player pressed Enter to confirm the selection
            if (Input.GetKeyDown(confirmKey))
            {
                // Update the current item based on the selected index
                if (selectedItemIndex == 0)
                {
                    currentItem = item1;
                }
                else
                {
                    currentItem = item2;
                }

                // Exit the selection mode
                isSelectingItem = false;
            }
        }
        else
        {
            // Check if the player pressed 1 or 2 to enter selection mode
            if (Input.GetKeyDown(selectItem1Key) || Input.GetKeyDown(selectItem2Key))
            {
                isSelectingItem = true;
            }
        }
    }

    // Method to add a new item to the inventory
    public void AddItem(Item newItem)
    {
        // If we already have two items, prompt the player to exchange one of them
        if (item1 != null && item2 != null)
        {
            // Enter selection mode to choose which item to exchange
            isSelectingItem = true;
        }
        // If we only have one item, add the new item to the second slot
        else if (item1 != null)
        {
            item2 = newItem;
        }
        // If we have no items, add the new item to the first slot
        else
        {
            item1 = newItem;
        }

        // Update the current item to the newly added item
        currentItem = newItem;
    }
}