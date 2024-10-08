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

    public void ConfirmReset(bool isConfirmed)
    {
        if (isConfirmed)
        {
            // If the player confirmed, delete all PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All data has been reset.");
        }

        // Hide the confirmation panel
        confirmationPanel.SetActive(false);
    }
}
