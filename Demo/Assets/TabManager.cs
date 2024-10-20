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
        currencyText.text = "Eternal Soul: " + PlayerPrefs.GetInt("PremiumCurrency").ToString();
        // Add listeners to buttons to handle tab switching
        eternalSoulTabButton.onClick.AddListener(() => SwitchTab(true));
        exclusiveItemTabButton.onClick.AddListener(() => SwitchTab(false));

        // Show Eternal Soul panel by default when the game starts
        SwitchTab(true);
    }

    private void Update()
    {
        currencyText.text = "Eternal Soul: " + PlayerPrefs.GetInt("PremiumCurrency").ToString();
    }
    // Function to switch between tabs
    public void SwitchTab(bool isEternalSoulTab)
    {
        eternalSoulPanel.SetActive(isEternalSoulTab);
        exclusiveItemPanel.SetActive(!isEternalSoulTab);

        // Debug.Log("Eternal Soul Panel Active: " + eternalSoulPanel.activeSelf);
        Debug.Log("Exclusive Item Panel Active: " + exclusiveItemPanel.activeSelf);
    }

}

