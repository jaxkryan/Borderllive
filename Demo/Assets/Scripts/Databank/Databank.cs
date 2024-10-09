using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Databank : MonoBehaviour
{
    public static Databank Instance;

    public GameObject itemUIPrefab;
    public GameObject powerupUIPrefab;
    [SerializeField] List<Item> items;
    [SerializeField] List<Powerups> powerups;
    public Transform contentPanel;  // Combined panel for both Items and Powerups

    private void Awake()
    {
        Instance = this;
    }

    // Display the list of items in the UI
    public void RefreshItemDisplay()
    {
        ClearContentPanel();  // Clear existing content

        foreach (Item item in items)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, contentPanel);
            itemUI.transform.Find("ItemName").GetComponent<Text>().text = item.nameLocalization.GetLocalizedString();
            itemUI.transform.Find("ItemDescription").GetComponent<Text>().text = item.descriptionLocalization.GetLocalizedString();
            itemUI.transform.Find("ItemImage").GetComponent<Image>().sprite = item.image;
            itemUI.transform.Find("ItemType").GetComponent<Text>().text = item.itemType.ToString();
            itemUI.transform.Find("ItemCooldown").GetComponent<Text>().text = $"CD: {item.cd}";
            itemUI.transform.Find("ItemHistoryDescription").GetComponent<Text>().text = item.historyDescriptionLocalization.GetLocalizedString();
        }

        // Refresh the layout of the content panel
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    // Display the list of powerups in the UI
    public void RefreshPowerupDisplay()
    {
        ClearContentPanel();  // Clear existing content

        foreach (Powerups powerup in powerups)
        {
            GameObject powerupUI = Instantiate(powerupUIPrefab, contentPanel);
            powerupUI.transform.Find("PowerupName").GetComponent<Text>().text = powerup.nameLocalization.GetLocalizedString();
            powerupUI.transform.Find("PowerupDescription").GetComponent<Text>().text = powerup.descriptionLocalization.GetLocalizedString();
            powerupUI.transform.Find("PowerupWeight").GetComponent<Text>().text = $"Weight: {powerup.Weight}";
        }

        // Refresh the layout of the content panel
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    // Clear all existing content from the panel
    private void ClearContentPanel()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);  // Clear old UI elements
        }
    }

}
