using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    public static List<Item> items = new List<Item>();

    // Initialize the database
    static ItemDatabase()
    {
        items.Add(Resources.Load<Item>("Items/Item1")); // Ensure correct path
        items.Add(Resources.Load<Item>("Items/Item2"));
        items.Add(Resources.Load<Item>("Items/Item3"));
        // Add other items here...

        Debug.Log("Items loaded into the database:");
        foreach (var item in items)
        {
            Debug.Log($"Item Name: {item.itemName}, Image: {item.image}");
        }
    }

    // Method to find an item by its name
    public static Item FindItemByName(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null; // Return null if not found
    }
}
