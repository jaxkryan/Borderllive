using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public List<Item> itemsForSale = new List<Item>();  // Items available in the shop
    public GameObject shopPanelPrefab;  // Prefab to display each shop panel in the UI
    public Transform shopContent;   // Parent object to hold the item buttons in the UI
    public OwnedActiveItem ownedActiveItem;  // Reference to OwnedActiveItem script
    public CurrencyManager currencyManager;  // Reference to the CurrencyManager script
    [SerializeField] Text currentSoulAmount;
    private PlayerController playerController;
    private void Start()
    {
        DisplayItems();
        playerController = FindObjectOfType<PlayerController>();
        currencyManager = playerController.GetComponent<CurrencyManager>();
        currentSoulAmount.text = ": " + currencyManager.currentAmount.ToString();
    }

    public void DisplayItems()
    {
        
        foreach (Item item in itemsForSale)
        {
            // Debug.Log("Displaying Items: " + itemsForSale.Count); 
            // Instantiate a ShopPanel instead of shopItemPrefab
            GameObject newShopPanel = Instantiate(shopPanelPrefab, shopContent);
            ShopItemUI shopItemUI = newShopPanel.GetComponent<ShopItemUI>();

            if (shopItemUI != null)
            {
                shopItemUI.SetItem(item, this);
                newShopPanel.SetActive(true);
                // Debug.Log("Item added to shop: " + item.itemName);
            }
            else
            {
                Debug.LogError("ShopItemUI component missing on shop panel prefab!");
            }
        }
    }


    // Handle item purchase
    public void PurchaseItem(Item item)
    {
        // Check if the player has enough currency to buy the item
        if (currencyManager.SpendCurrency(item.cost))
        {
            // Add the item to OwnedActiveItem
            ownedActiveItem.AddItem(item);
            Debug.Log("Purchased: " + item.itemName);
        }
        else
        {
            Debug.Log("Not enough currency to buy: " + item.itemName);
        }
        currentSoulAmount.text = ": " + currencyManager.currentAmount.ToString();

    }
}
