using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ExchangeUI : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private XPTracker xPTracker;
    [SerializeField] Slider soulToXPExchangeSlider;
    [SerializeField] TMP_Text soulAmountText;
    [SerializeField] TMP_Text xpAmountText;
    [SerializeField] GameObject interactMessage;
    [SerializeField] GameObject exchangePanel; // The panel that contains the slider UI

    private int currentPlayerSouls;
    private int xpToAdd;

    public static bool isExchangePanelActive; // Add this flag
    public LocalizedString localizedSoulAmountText; // Spend: X souls
    public LocalizedString localizedXPAmountText;   // for Y XP?
    public LocalizedString localizedConfirmText;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        xPTracker = FindObjectOfType<XPTracker>();
        exchangePanel.SetActive(false); // Hide the exchange panel at the start
        isExchangePanelActive = false; // Initially set the panel state as inactive
        UpdateLocalizedText();

    }

    // Call this function to open the exchange UI
    public void OpenExchangeUI()
    {
        Time.timeScale = 0;
        interactMessage.SetActive(false);
        currentPlayerSouls = currencyManager.currentAmount;
        soulToXPExchangeSlider.wholeNumbers = true;
        soulToXPExchangeSlider.maxValue = currencyManager.currentAmount;
        soulToXPExchangeSlider.value = 0; // Start the slider at 0
        soulAmountText.text = "0 Soul";
        xpAmountText.text = "0 XP";
        exchangePanel.SetActive(true); // Show the panel
        isExchangePanelActive = true;  // Set flag to indicate panel is active
    }

    private void Update()
    {
        // Update the soul amount text based on the slider's value
        localizedSoulAmountText.StringChanged += (string localizedValue) =>
        {
            soulAmountText.text = string.Format(localizedValue, soulToXPExchangeSlider.value);
        };

        localizedXPAmountText.StringChanged += (string localizedValue) =>
        {
            xpAmountText.text = string.Format(localizedValue, soulToXPExchangeSlider.value * 2);
        };

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
            currencyManager.SpendCurrency(soulsToExchange); // Deduct the souls
            xpToAdd = soulsToExchange * 2; // Example: 1 soul = 2 XP, adjust as needed
            xPTracker.AddXP(xpToAdd); // Add XP to the player's XP tracker

            Debug.Log("Exchanged " + soulsToExchange + " souls for " + xpToAdd + " XP.");
        }
        interactMessage.SetActive(true);
        exchangePanel.SetActive(false); // Hide the panel if no souls are exchanged
        isExchangePanelActive = false; // Set flag to false when UI is closed
    }

        private void UpdateLocalizedText()
    {
        // Assign the localized text to UI elements
        soulAmountText.text = localizedSoulAmountText.GetLocalizedString();
        xpAmountText.text = localizedXPAmountText.GetLocalizedString();
    }
}


