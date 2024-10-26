using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Databank : MonoBehaviour
{
    public static Databank Instance;

    public GameObject itemUIPrefab;
    public GameObject powerupUIPrefab;
    public GameObject npcUIPrefab; // Prefab for NPC button
    [SerializeField] List<Item> items;
    [SerializeField] List<Powerups> powerups;
    [SerializeField] List<Elemental_NPC> npcs; // List of NPCs
    public Transform contentPanel; // Combined panel for Items and Powerups
    public GameObject npcPanel; // Panel that holds NPC buttons
    public GameObject npcDescriptionPanel; // Panel for displaying NPC details

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Optionally open default tab, e.g., OpenItemTab();
    }

    public void OpenItemTab()
    {
        HideAllPanels();  // Ensure all panels are hidden
        contentPanel.gameObject.SetActive(true);  // Show content panel for items
        RefreshItemDisplay();
    }

    public void OpenPowerupTab()
    {
        HideAllPanels();  // Ensure all panels are hidden
        contentPanel.gameObject.SetActive(true);  // Show content panel for powerups
        RefreshPowerupDisplay();
    }

    public void OpenNPCTab()
    {
        HideAllPanels();  // Ensure all panels are hidden
        npcPanel.SetActive(true);  // Show NPC button panel
        npcDescriptionPanel.SetActive(true);  // Show NPC description panel
        PopulateNPCButtons();
    }

    private void HideAllPanels()
    {
        contentPanel.gameObject.SetActive(false);
        npcPanel.SetActive(false);
        npcDescriptionPanel.SetActive(false);

        ClearContentPanel();
        ClearNPCPanel();
        ClearNPCDescriptionPanel();
    }


    // Display the list of items in the UI
    public void RefreshItemDisplay()
    {
        ClearContentPanel(); // Clear existing content

        foreach (Item item in items)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, contentPanel);
            itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.nameLocalization.GetLocalizedString();
            itemUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.descriptionLocalization.GetLocalizedString();
            itemUI.transform.Find("ItemImage").GetComponent<Image>().sprite = item.image;
            itemUI.transform.Find("ItemType").GetComponent<TextMeshProUGUI>().text = item.itemType.ToString();
            itemUI.transform.Find("ItemCooldown").GetComponent<TextMeshProUGUI>().text = $"CD: {item.cd}";
            itemUI.transform.Find("Scroll View/Viewport/Content/ItemHistoryDescription").GetComponent<TextMeshProUGUI>().text = item.historyDescriptionLocalization.GetLocalizedString();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    // Display the list of powerups in the UI
    public void RefreshPowerupDisplay()
    {
        ClearContentPanel(); // Clear existing content

        foreach (Powerups powerup in powerups)
        {
            GameObject powerupUI = Instantiate(powerupUIPrefab, contentPanel);
            powerupUI.transform.Find("PowerupWeight").GetComponent<TextMeshProUGUI>().text = $"Weight: {powerup.Weight}";
            powerupUI.transform.Find("PowerupName").GetComponent<TextMeshProUGUI>().text = powerup.nameLocalization.GetLocalizedString();
            powerupUI.transform.Find("PowerupDescription").GetComponent<TextMeshProUGUI>().text = powerup.descriptionLocalization.GetLocalizedString();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    // Clear all existing content from the content panel
    private void ClearContentPanel()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject); // Clear old UI elements
        }
    }

    // Clear all existing NPC buttons from the NPC panel
    private void ClearNPCPanel()
    {
        foreach (Transform child in npcPanel.transform)
        {
            Destroy(child.gameObject); // Clear old NPC buttons
        }
    }

    // Clear NPC description panel content
    private void ClearNPCDescriptionPanel()
    {
        npcDescriptionPanel.transform.Find("NPCUIPrefab/NPCNameDetail").GetComponent<TextMeshProUGUI>().text = "";
        npcDescriptionPanel.transform.Find("NPCUIPrefab/Scroll View/Viewport/Content/NPCDescriptionDetail").GetComponent<TextMeshProUGUI>().text = "";
    }

    private void PopulateNPCButtons()
    {
        ClearNPCPanel(); // Clear existing NPC buttons if needed

        foreach (Elemental_NPC npc in npcs)
        {
            GameObject npcButton = Instantiate(npcUIPrefab, npcPanel.transform);
            npcButton.transform.Find("NPCName").GetComponent<TextMeshProUGUI>().text = npc.nameLocalization.GetLocalizedString();
            npcButton.GetComponent<Button>().onClick.AddListener(() => ShowNPCDetails(npc));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(npcPanel.GetComponent<RectTransform>());
    }

    private void ShowNPCDetails(Elemental_NPC npc)
    {
        npcDescriptionPanel.transform.Find("NPCUIPrefab/NPCNameDetail").GetComponent<TextMeshProUGUI>().text = npc.nameLocalization.GetLocalizedString();
        npcDescriptionPanel.transform.Find("NPCUIPrefab/Scroll View/Viewport/Content/NPCDescriptionDetail").GetComponent<TextMeshProUGUI>().text = npc.descriptionLocalization.GetLocalizedString();
    }
}
