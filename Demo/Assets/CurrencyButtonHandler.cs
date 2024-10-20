using UnityEngine;
using UnityEngine.UI;

public class CurrencyButtonHandler : MonoBehaviour
{
    public int currencyValue; // value of the currency associated with this button
    // public string currencyCode; // code associated with this button
    public CurrencyInputHandler currencyInputHandler;
    public GameObject codeInputPanel; // the panel that contains the input field

    private void Start()
    {
        // Add a listener to the button
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        // Show the code input panel
        codeInputPanel.SetActive(true);

        // Set the currency value and code in the CurrencyInputHandler script
        currencyInputHandler.SetCurrencyValue(currencyValue);
        // currencyInputHandler.SetCurrencyCode(currencyCode);
    }
}