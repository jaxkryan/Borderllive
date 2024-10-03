using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string itemType;
    public float cd;
    public string imageName; // Field for storing the image name
}


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

    // References to the Image components of HUD items
    public Image item1Image;
    public Image item2Image;

    //3rd item for exchange
    public GameObject exchangeScreen; // The exchange UI screen
    public Button item1Button;        // Button to exchange with item1
    public Button item2Button;        // Button to exchange with item2
    public Button cancelButton;       // Button to cancel the exchange
    private Item newItem;            // Temporary storage for the new item being purchased

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

                // Update the UI images
                UpdateUI();

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
            exchangeScreen.SetActive(true); // Show the exchange UI
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

        // Update the UI images
        UpdateUI();
    }

    public void SelectItemToExchange(int index)
    {
        if (index == 0)
        {
            item1 = newItem;  // Replace item1
        }
        else if (index == 1)
        {
            item2 = newItem;  // Replace item2
        }

        exchangeScreen.SetActive(false);
        UpdateUI();
    }

        public void CancelExchange()
    {
        newItem = null; // Clear the new item reference
        exchangeScreen.SetActive(false); // Hide the exchange UI
    }

    public void SaveItems()
    {
        if (item1 != null)
        {
            string item1Json = JsonUtility.ToJson(new ItemData
            {
                itemName = item1.itemName,
                itemType = item1.itemType.ToString(),
                cd = item1.cd,
                imageName = item1.GetImageName() // Save image name
            });
            PlayerPrefs.SetString("Item1", item1Json);
        }

        if (item2 != null)
        {
            string item2Json = JsonUtility.ToJson(new ItemData
            {
                itemName = item2.itemName,
                itemType = item2.itemType.ToString(),
                cd = item2.cd,
                imageName = item2.GetImageName() // Save image name
            });
            PlayerPrefs.SetString("Item2", item2Json);
        }

        PlayerPrefs.Save();
    }

    public void LoadItems()
    {
        if (PlayerPrefs.HasKey("Item1"))
        {
            string item1Json = PlayerPrefs.GetString("Item1");
            ItemData item1Data = JsonUtility.FromJson<ItemData>(item1Json);
            item1 = ItemDatabase.FindItemByName(item1Data.itemName);
            if (item1 != null)
            {
                item1.cd = item1Data.cd;
                item1.image = LoadImage(item1Data.imageName); // Load image by name
            }
        }

        if (PlayerPrefs.HasKey("Item2"))
        {
            string item2Json = PlayerPrefs.GetString("Item2");
            ItemData item2Data = JsonUtility.FromJson<ItemData>(item2Json);
            item2 = ItemDatabase.FindItemByName(item2Data.itemName);
            if (item2 != null)
            {
                item2.cd = item2Data.cd;
                item2.image = LoadImage(item2Data.imageName); // Load image by name
            }
        }

        UpdateUI(); // Update UI to reflect loaded items
    }

    private Sprite LoadImage(string imageName)
    {
        if (string.IsNullOrEmpty(imageName))
        {
            return null; // Return null if the image name is empty
        }
        return Resources.Load<Sprite>("Items/ItemImage/" + imageName); // Adjust the path if needed
    }


    private void Start(){
                item1Button.onClick.AddListener(() => SelectItemToExchange(0));
        item2Button.onClick.AddListener(() => SelectItemToExchange(1));
        cancelButton.onClick.AddListener(CancelExchange);

        // Hide exchange screen initially
        exchangeScreen.SetActive(false);
        UpdateUI();
    }
    // Method to update the UI images
    public void UpdateUI()
    {
        if (item1 != null && item1Image != null)
        {
            item1Image.sprite = item1.image; // Assuming item1 has a sprite property called itemSprite
        }

        if (item2 != null && item2Image != null)
        {
            item2Image.sprite = item2.image; // Assuming item2 has a sprite property called itemSprite
        }
    }
}
