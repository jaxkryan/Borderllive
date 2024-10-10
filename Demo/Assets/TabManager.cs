using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject eternalSoulPanel;  // Panel for Eternal Soul
    public GameObject exclusiveItemPanel;  // Panel for Exclusive Item
    public Button eternalSoulTabButton;  // Button for Eternal Soul Tab
    public Button exclusiveItemTabButton;  // Button for Exclusive Item Tab

    public TextMeshProUGUI currencyText;

    private void Start()
    {
        int currentCurrency = PlayerPrefs.GetInt("PremiumCurrency");
        currencyText.text = "Premium Currency: " + PlayerPrefs.GetInt("PremiumCurrency").ToString();
        // Add listeners to buttons to handle tab switching
        eternalSoulTabButton.onClick.AddListener(() => SwitchTab(true));
        exclusiveItemTabButton.onClick.AddListener(() => SwitchTab(false));

        // Show Eternal Soul panel by default when the game starts
        SwitchTab(true);
    }

    // Function to switch between tabs
    public void SwitchTab(bool isEternalSoulTab)
    {
        // Toggle panel visibility based on the selected tab
        eternalSoulPanel.SetActive(isEternalSoulTab);
        exclusiveItemPanel.SetActive(!isEternalSoulTab);

        // Optionally, highlight the active tab button (optional cosmetic)
        // eternalSoulTabButton.interactable = !isEternalSoulTab;
        // exclusiveItemTabButton.interactable = isEternalSoulTab;
    }
}

