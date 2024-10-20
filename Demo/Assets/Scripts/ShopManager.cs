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

    public GameObject shopPanel;
    public OwnedActiveItem ownedActiveItem;  // Reference to OwnedActiveItem script
    public CurrencyManager currencyManager;  // Reference to the CurrencyManager script
    [SerializeField] Text currentSoulAmount;
    private PlayerController playerController;
    private ShopNPC shopNPC; //hop
    private void Start()
    {
        DisplayItems();
        playerController = FindObjectOfType<PlayerController>();
        currencyManager = playerController.GetComponent<CurrencyManager>();
        currentSoulAmount.text = ": " + currencyManager.currentAmount.ToString();
        shopNPC = FindObjectOfType<ShopNPC>();
    }

    public void DisplayItems()
    {
        List<Item> enableItem = new List<Item>();

        // Filter out enabled items
        foreach (Item i in itemsForSale)
        {
            if (i.isEnable == true) enableItem.Add(i);
        }

        // Shuffle the list to randomize the order of enabled items
        ShuffleList(enableItem);

        // Limit the number of items to display (2 or 3)
        int numItemsToDisplay = Mathf.Min(3, enableItem.Count);  // Choose the smaller of 3 or the number of items in the list

        // Loop through the selected items and display them
        for (int i = 0; i < numItemsToDisplay; i++)
        {
            Item item = enableItem[i];

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

    // Helper function to shuffle the list
    private void ShuffleList(List<Item> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Item temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void PurchaseItem(Item item, ShopItemUI shopItemUI)
    {
        // Check if the player has enough currency to buy the item
        if (currencyManager.SpendCurrency(item.cost))
        {
            // Add the item to OwnedActiveItem
            ownedActiveItem.AddItem(item);

            // Hide the item after purchase
            shopItemUI.gameObject.SetActive(false); // Hide the purchased item

            // Check if any other shop items are still active
            CheckAndDisableShopPanel();
        }
        else
        {
            Debug.Log("Not enough currency to buy: " + item.itemName);
        }

        // Update the soul amount in the UI
        currentSoulAmount.text = ": " + currencyManager.currentAmount.ToString();
    }
    private void CheckAndDisableShopPanel()
    {
        bool hasActiveItems = false;

        // Loop through all children of shopContent to check if any are still active
        foreach (Transform shopItem in shopContent)
        {
            if (shopItem.gameObject.activeSelf)
            {
                hasActiveItems = true;
                break;  // Exit the loop as soon as we find one active item
            }
        }

        // If no active items are found, disable the entire shop panel
        if (!hasActiveItems)
        {
            shopNPC.CloseShop();
            //shopPanel.SetActive(false);
            Debug.Log("All items purchased. Shop panel disabled.");
        }
    }


}