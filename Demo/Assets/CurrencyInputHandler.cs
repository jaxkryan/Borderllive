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

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            gameObject.SetActive(false);
        }
    }
    public void SubmitCode()
    {
        
        // Get the input code from the input field
        string inputCode = inputField.text;
        inputField.text = "";
        // Validate the input code using the StringValidator
        if (!ValidateString(inputCode))
        {
            Debug.Log("Input code is invalid based on the validation rules.");
            return; // Exit early if the code is invalid
        }

        // Check if the input code matches the currency code
        else
        {
            Debug.Log("processing value: " + currencyValue);
            if (PlayerPrefs.HasKey(inputCode))
            {   
                Debug.Log("This code has already been used.");
                return; // Exit early if the code has already been redeemed
            }
            // Check if the "PremiumCurrency" key exists in PlayerPrefs
            if (!PlayerPrefs.HasKey("PremiumCurrency"))
            {
                // If it doesn't exist, set it to 0
                PlayerPrefs.SetInt("PremiumCurrency", 0);
            }
            Debug.Log("code " + inputCode + " is " + PlayerPrefs.GetInt(inputCode));
            // Get the current currency value
            int currentCurrency = PlayerPrefs.GetInt("PremiumCurrency");
            // if (PlayerPrefs.GetInt(currencyCode)==1) return;
            // Add the new currency value
            PlayerPrefs.SetInt("PremiumCurrency", currentCurrency + currencyValue);

            // Display the updated currency on the TextMeshProUGUI
            currencyText.text = "Premium Currency: " + PlayerPrefs.GetInt("PremiumCurrency").ToString();
            PlayerPrefs.SetInt(inputCode, 1);
            gameObject.SetActive(false);
        }

    }

    public static bool ValidateString(string input)
    {
        if (input.Length != 10)
        {
            Debug.Log(1);
            return false;
        }

        // Rule 1: First character is from '0' to '9' and 'a' to 'h'
        if (!"0123456789abcdefgh".Contains(input[0]))
        {
            Debug.Log(2);
            return false;
        }

        // Rule 2: Second character is from 'i' to 'v' and '4' to '9'
        if (!"iijklmnopqrstuv456789".Contains(input[1]))
        {
            Debug.Log(3);
            return false;
        }

        // Rule 3: Third character is from the list '0', '3', '5', '8', 'k', 'm', 'w', 'i', 'p', 'l', 'c', 'b', 'a', 'y', 't'
        if (!"0358kmwiplcbyta".Contains(input[2]))
        {
            Debug.Log(4);
            return false;
        }

        // Rule 4: Fourth character is from 'a' to 'z'
        if (!"abcdefghijklmnopqrstuvwxyz".Contains(input[3]))
        {
            Debug.Log(5);
            return false;
        }

        // Rule 5: Fifth character is from 'f' to 'm'
        if (!"fghijklm".Contains(input[4]))
        {
            Debug.Log(6);
            return false;
        }

        // Rule 6: Sixth character is from '0' to '7'
        if (!"01234567".Contains(input[5]))
        {
            Debug.Log(7);
            return false;
        }

        // Rule 7: Eighth character is from the list 'q', 'w', 'e', 'r', 't', 'g', 'f', 'd', 's', 'a', 'b', 'v', 'c', 'x', 'z', '2', '3', '5', '6', '7', '9'
        if (!"qwertgfdsabvcxz235679".Contains(input[6]))
        {
            Debug.Log(8);
            return false;
        }

        // Rule 8: Ninth character is from the list 'p', 'o', 'i', 'u', 'y', 'l', 'k', 'j', 'h', 'g', 'm', 'n', 'b', 'v', '3', '5', '7', '8', '1'
        if (!"poiuylkjhgmnbv35781".Contains(input[7]))
        {
            Debug.Log(9);
            return false;
        }

        // Rule 9: Tenth character is from '1' to '9'
        if (!"123456789".Contains(input[8]))
        {
            Debug.Log(10);
            return false;
        }
        if (!"0123456789abcdefgh".Contains(input[9]))
        {
            Debug.Log(11);
            return false;
        }

        return true;
    }

}
