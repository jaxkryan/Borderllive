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
        itemImage.sprite = item.image;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        costText.text = "Exchange and receive (" + item.cost.ToString() + " souls)";


        exchangeButton.onClick.RemoveAllListeners();
        exchangeButton.onClick.AddListener(OnExchangeButtonClicked);
    }

    private void OnExchangeButtonClicked()
    {
        if (currentItem == null)
        {
            Debug.Log("No item selected for exchange.");
            return;
        }

        // Here you can open a dialog/UI to choose between exchanging item1 or item2
        // Debug.Log("Exchanging " + currentItem.itemName);

        // Call the exchange method in ItemManagement
        itemManager.ExchangeItem(currentItem);
    }
}
