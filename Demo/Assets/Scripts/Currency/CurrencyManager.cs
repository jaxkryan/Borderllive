using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public int startingAmount = 50; // Starting amount of money for the player
    public int currentAmount; // Current money of the player
    public Text currencyText; // Reference to the UI Text element to display the money

    private void Awake()
    {
        currentAmount = startingAmount;
        UpdateCurrencyText(); // Set initial money display
    }
    void Start()
    {
    }

    // Add money
    public void AddCurrency(int amount)
    {
        currentAmount += amount;
        UpdateCurrencyText(); // Update the display every time money is added
    }

    // Spend money
    public bool SpendCurrency(int amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;
            UpdateCurrencyText(); // Update the display every time money is spent
            return true;
        }
        else
        {
            Debug.Log("Not enough soul!");
            return false; // Indicate the player does not have enough money
        }
    }

    // Update the money text UI directly
    void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = ": " + currentAmount.ToString();
        }
        else
        {
            Debug.LogError("Money text UI element not assigned!");
        }
    }
    public void SetCurrency(int amount)
    {
        currentAmount = amount;
        UpdateCurrencyText(); // Assuming you update the UI or other elements when the money is changed
    }

}
