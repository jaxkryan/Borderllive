using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    // UI elements to display item information
    public Image itemImage;        // To display the item's image
    public Text itemNameText;  // To display the item's name
    public Text itemDescriptionText;  // To display the item's description
    public Button exchangeButton;  // Button to purchase the item
    public Text costText;      // To display the cost on the purchase button
    private Item currentItem;      // The item that this UI is displaying
    private ItemManagement itemManager;  // Reference to itemManager to handle the purchase

    // This method sets the UI elements based on the item data
    public void SetItem(Item item, ItemManagement itemManager)
    {
        this.itemManager = itemManager;
        this.currentItem = item;
        // Set UI elements
        itemImage.sprite = item.image;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        costText.text = "Buy (" + item.cost.ToString() + " Souls)";

        // Add listener to the purchase button
        exchangeButton.onClick.RemoveAllListeners();  // Clear any existing listeners
        exchangeButton.onClick.AddListener(OnExchangeButtonClicked);
    }

    // // Method called when the purchase button is clicked
    private void OnExchangeButtonClicked()
    {
        if (currentItem == null) {
            Debug.Log("ci is null");
             return;
        }
        else{
            Debug.Log("info: " + currentItem.itemDescription);
        }
        // Tell the itemManager to handle the purchase
         itemManager.ExchangeItem(currentItem, this);
    }
}
