using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : MonoBehaviour
{
    public ShopManager shopManager;
    public GameObject shopPanel;
    public static PurchasePanel instance; // Singleton instance
    public Button exitButton;
    public Text codeInputFieldTxt; // Assign the input field in the inspector
    public Button confirmButton; // Assign the confirm button in the inspector
    private Item currentItem;

    private void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        // Get the PurchaseContent panel
        Transform purchaseContent = transform.Find("PurchaseContent");
        // Get the CodeInputField
        Transform codeInputField = purchaseContent.Find("CodeInputField");
        // Get the InputFieldText component
        codeInputFieldTxt = codeInputField.Find("InputFieldTxt").GetComponent<Text>();

        codeInputFieldTxt.text = String.Empty;
    }

    public void Show(Item item, GameObject currentPanel)
    {
        currentItem = item;
        shopPanel = currentPanel;

        // Show the purchase panel
        gameObject.SetActive(true);
    }

    public void OnConfirmButtonClick()
    {
        // Get the entered code
        String inputCode = codeInputFieldTxt.text;

        Debug.Log("enter : " + inputCode);
        Debug.Log("code : " + currentItem.code);

        // Validate the code (e.g., check against a database or a predefined list)
        if (String.Equals(inputCode, currentItem.code))
        {
            currentItem.isEnable = true;
            // Purchase the item (e.g., deduct coins, add item to inventory)
            //PurchaseItem(itemId);
            // Hide the purchase panel
            gameObject.SetActive(false);
            shopPanel.SetActive(false);
        }
        else
        {
            // Display an error message (e.g., "Invalid code")
            Debug.Log("Invalid code");
        }
    }

    public void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}