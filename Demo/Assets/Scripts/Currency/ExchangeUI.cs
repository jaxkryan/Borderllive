using UnityEngine;
using UnityEngine.UI;

public class ExchangeUI : MonoBehaviour
{
    public Slider soulToXPExchangeSlider;
    public Text soulAmountText;
    public GameObject exchangePanel; // The panel that contains the slider UI

    private int currentPlayerSouls;
    private int xpToAdd;

    private void Start()
    {
        exchangePanel.SetActive(false); // Hide the exchange panel at the start
    }

    // Call this function to open the exchange UI
    public void OpenExchangeUI()
    {
        Time.timeScale = 0;
        currentPlayerSouls = CurrencyManager.Instance.currentAmount;
        soulToXPExchangeSlider.wholeNumbers = true;
        soulToXPExchangeSlider.maxValue = CurrencyManager.Instance.currentAmount;
        soulToXPExchangeSlider.value = 0; // Start the slider at 0
        soulAmountText.text = "Spend: 0 for 0 XP?";
        exchangePanel.SetActive(true); // Show the panel
    }

    private void Update()
    {
        // Update the soul amount text based on the slider's value
        soulAmountText.text = "Spend: " + soulToXPExchangeSlider.value.ToString() + " for " + (soulToXPExchangeSlider.value * 2).ToString() + " XP?";

        // Check for Enter key press to confirm exchange
        if (exchangePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmExchange();
        }
    }

    // Called when the player confirms the exchange
    private void ConfirmExchange()
    {
        Time.timeScale = 1;
        int soulsToExchange = (int)soulToXPExchangeSlider.value;

        if (soulsToExchange > 0)
        {
            CurrencyManager.Instance.SpendCurrency(soulsToExchange); // Deduct the souls
            xpToAdd = soulsToExchange * 2; // Example: 1 soul = 2 XP, adjust as needed
            XPTracker.Instance.AddXP(xpToAdd); // Add XP to the player's XP tracker

            Debug.Log("Exchanged " + soulsToExchange + " souls for " + xpToAdd + " XP.");
            exchangePanel.SetActive(false); // Hide the panel after the exchange
        }
        else
        {
            exchangePanel.SetActive(false); // Hide the panel if no souls are exchanged
        }
    }
}
