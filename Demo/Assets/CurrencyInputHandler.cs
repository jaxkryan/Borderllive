using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyInputHandler : MonoBehaviour
{
public TMP_InputField inputField;
    public Button submitButton;
    public TextMeshProUGUI currencyText;
    private int currencyValue; // value of the currency associated with the current button click
    private string currencyCode; // code associated with the current button click

    public void SetCurrencyValue(int value)
    {
        currencyValue = value;
    }

    public void SetCurrencyCode(string code)
    {
        currencyCode = code;
    }

    private void Start()
    {
        // Add a listener to the submit button
        submitButton.onClick.AddListener(SubmitCode);
    }

    public void SubmitCode()
    {
        // Get the input code from the input field
        string inputCode = inputField.text;
        // Debug.Log(inputCode + " = " + currencyCode);
        // Check if the input code matches the currency code
        if (inputCode.Equals(currencyCode))
        {
            // Check if the "PremiumCurrency" key exists in PlayerPrefs
            if (!PlayerPrefs.HasKey("PremiumCurrency"))
            {
                // If it doesn't exist, set it to 0
                PlayerPrefs.SetInt("PremiumCurrency", 0);
            }

            // Get the current currency value
            int currentCurrency = PlayerPrefs.GetInt("PremiumCurrency");

            // Add the new currency value
            PlayerPrefs.SetInt("PremiumCurrency", currentCurrency + currencyValue);

            // Display the currency on the TextMeshProUGUI
            currencyText.text = "Premium Currency: " + PlayerPrefs.GetInt("PremiumCurrency").ToString();
        }
        else
        {
            // Handle invalid input code
            Debug.Log("Invalid input code");
        }
    }
}