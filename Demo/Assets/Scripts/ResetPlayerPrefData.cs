using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefData : MonoBehaviour
{
    public GameObject confirmationPanel; // Reference to your confirmation panel

    void Start()
    {
        confirmationPanel.SetActive(false); // Make sure the panel is hidden at the start
    }

    public void OnResetButtonPressed()
    {
        // Show the confirmation panel
        confirmationPanel.SetActive(true);
    }

    private const string PlayerPrefsKeyPrefix = "ItemState_";
    private string[] itemNames = { "item1", "item2", "item3", "item4", "item5", "item6", "item7" }; // Add more if needed

    public void ConfirmReset(bool isConfirmed)
    {
        if (isConfirmed)
        {
            // Store all item states
            Dictionary<string, int> itemStates = new Dictionary<string, int>();

            foreach (var itemName in itemNames)
            {
                string key = PlayerPrefsKeyPrefix + itemName;
                if (PlayerPrefs.HasKey(key))
                {
                    itemStates[key] = PlayerPrefs.GetInt(key);
                }
            }

            int premiumCurrency = PlayerPrefs.GetInt("PremiumCurrency", 0);
            Debug.Log(premiumCurrency);
            PlayerPrefs.DeleteAll();

            // Restore item states
            foreach (var itemState in itemStates)
            {
                PlayerPrefs.SetInt(itemState.Key, itemState.Value);
            }

            // After PlayerPrefs.DeleteAll(), restore the saved premiumCurrency value.
            PlayerPrefs.SetInt("PremiumCurrency", premiumCurrency);

            // Save again to persist restored values
            PlayerPrefs.Save();

            Debug.Log("All data has been reset.");
        }

        // Hide the confirmation panel
        confirmationPanel.SetActive(false);
    }
}
