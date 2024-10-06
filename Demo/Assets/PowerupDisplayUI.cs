using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupDisplayUI : MonoBehaviour
{
    public GameObject powerupPanel; // Reference to the panel that shows the power-ups
    public GameObject powerupPrefab; // Prefab that displays each power-up

    public GameObject noPowerup;
    public Transform contentParent; // The content container where power-ups are displayed
    private OwnedPowerups ownedPowerups; // Reference to the player's owned power-ups
    private bool isPanelVisible = false; // To track if the power-up panel is visible or not

    private void Start()
    {
        // Get reference to OwnedPowerups component
        ownedPowerups = FindObjectOfType<OwnedPowerups>();

        if (ownedPowerups == null)
        {
            Debug.LogError("OwnedPowerups component not found!");
            return;
        }
        TogglePowerupPanel();
        // // Initially hide the powerup panel
        // powerupPanel.SetActive(false);
    }

    private void Update()
    {
        if (ownedPowerups.activePowerups.Count == 0) noPowerup.SetActive(true);
        else noPowerup.SetActive(false);
        UpdatePowerupDisplay();
    }

    private void TogglePowerupPanel()
    {
        isPanelVisible = !isPanelVisible;
        powerupPanel.SetActive(isPanelVisible);

        if (isPanelVisible)
        {
            // Update the UI with power-up information
            UpdatePowerupDisplay();
        }
    }

    private void UpdatePowerupDisplay()
    {
        // Clear previous power-up entries
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Populate the UI with the current power-ups
        foreach (Powerups powerup in ownedPowerups.activePowerups)
        {
            // Instantiate a new power-up UI prefab
            GameObject powerupItem = Instantiate(powerupPrefab, contentParent);
            // Set the name and description in the UI
            // Try to find the child objects
            Transform nameTextTransform = powerupItem.transform.Find("PowerupNameText");
            Transform descriptionTextTransform = powerupItem.transform.Find("PowerupDescriptionText");

            Text nameText = nameTextTransform.GetComponent<Text>();
            Text descriptionText = descriptionTextTransform.GetComponent<Text>();

            nameText.text = powerup.nameLocalization.GetLocalizedString();
            descriptionText.text = powerup.descriptionLocalization.GetLocalizedString();
        }
    }

}
