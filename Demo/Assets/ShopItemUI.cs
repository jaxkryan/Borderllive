using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopItemUI : MonoBehaviour
{
    // UI elements to display item information
    public Image itemImage;        // To display the item's image
    public Text itemNameText;  // To display the item's name
    public Text itemDescriptionText;  // To display the item's description
    public Button purchaseButton;  // Button to purchase the item
    public Text costText;      // To display the cost on the purchase button
    private Item currentItem;      // The item that this UI is displaying
    private ShopManager shopManager;  // Reference to ShopManager to handle the purchase

    // This method sets the UI elements based on the item data
    public void SetItem(Item item, ShopManager shopManager)
    {
        this.shopManager = shopManager;
        this.currentItem = item;
        // Set UI elements
        itemImage.sprite = item.image;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        costText.text = "Buy (" + item.cost.ToString() + " Souls)";

        // Add listener to the purchase button
        purchaseButton.onClick.RemoveAllListeners();  // Clear any existing listeners
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

    // Method called when the purchase button is clicked
    private void OnPurchaseButtonClicked()
    {
        if (currentItem == null) {
           // Debug.Log("ci is null");
             return;
        }
        // else{
        //     Debug.Log("info: " + currentItem.itemDescription);
        // }
        // Tell the ShopManager to handle the purchase
         shopManager.PurchaseItem(currentItem, this);
    }
}