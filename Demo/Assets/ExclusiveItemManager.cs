using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExclusiveItemManager : MonoBehaviour
{
    public GameObject itemPrefab;  // Reference to the item UI prefab (which is now a button)
    public Transform exclusiveItemPanel;  // The panel to display items in (with a HorizontalLayoutGroup)
    public List<Item> items;  // List of items to display

    public GameObject confirmationPanel;  // The panel for confirmation
    public TextMeshProUGUI confirmationText;  // Text to display the confirmation message
    public Button unlockButton;  // Button to confirm unlock
    private Item selectedItem;  // The currently selected item

    private void Start()
    {
        DisplayItems();

        // Initially hide the confirmation panel
        confirmationPanel.SetActive(false);
    }

    // Method to display items in the exclusive item panel
    public void DisplayItems()
    {
        // Clear the panel first
        foreach (Transform child in exclusiveItemPanel)
        {
            Destroy(child.gameObject);
        }

        // Loop through all items and display them as buttons
        foreach (Item item in items)
        {
            // Instantiate the item UI prefab
            GameObject itemUI = Instantiate(itemPrefab, exclusiveItemPanel);

            // Set the item image, name, description, and unlock cost
            itemUI.transform.Find("Image").GetComponent<Image>().sprite = item.image;
            itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.nameLocalization.GetLocalizedString();
            itemUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.descriptionLocalization.GetLocalizedString();
            itemUI.transform.Find("UnlockCost").GetComponent<TextMeshProUGUI>().text = $"Unlock Cost: {item.unlockCost}";

            // Add button functionality
            Button itemButton = itemUI.GetComponent<Button>();
            itemButton.onClick.AddListener(() => OnItemClick(item));

            Debug.Log($"Displaying item: {item.itemName} with unlock cost: {item.unlockCost}");
        }
    }


    // Method called when an item button is clicked
    public void OnItemClick(Item item)
    {
        // Store the selected item
        selectedItem = item;

        // Update the confirmation text with the item's info
        confirmationText.text = $"Do you want to unlock {item.nameLocalization.GetLocalizedString()} for {item.unlockCost} souls?";

        // Show the confirmation panel
        confirmationPanel.SetActive(true);

        // Set up the unlock button to handle unlocking
        unlockButton.onClick.RemoveAllListeners();  // Clear any previous listeners
        unlockButton.onClick.AddListener(UnlockItem);  // Add new listener for this item
    }

    // Method to unlock the selected item
    public void UnlockItem()
    {
        if (selectedItem != null)
        {
            // Unlock logic (you can update PlayerPrefs or any other mechanism)
            Debug.Log($"Item {selectedItem.nameLocalization.GetLocalizedString()} unlocked!");

            // Optionally, update the UI or PlayerPrefs here to reflect the unlocked item
            selectedItem.isEnable = true;

            // Hide the confirmation panel
            confirmationPanel.SetActive(false);
        }
    }

    // Optionally, add a cancel method for the cancel button
    public void CancelUnlock()
    {
        confirmationPanel.SetActive(false);
    }
}
