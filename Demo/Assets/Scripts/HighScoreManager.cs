using System.Collections;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI elapsedTimeText; // Assign this in the Inspector
    public Color normalColor = Color.white; // Color for normal display
    public Color eventColor = Color.red;    // Color for event display

    private const string PlayerDiedKey = "PlayerDied"; // Key for tracking player death
    private const string ElapsedTimeKey = "ElapsedTime"; // Key for storing elapsed time as string

    void Start()
    {
        // Load the elapsed time string from PlayerPrefs
        if (PlayerPrefs.HasKey(ElapsedTimeKey))
        {
            string elapsedTime = PlayerPrefs.GetString(ElapsedTimeKey);
            if (elapsedTimeText != null)
            {
                // Check if the player is playing an event
                bool isInEvent = PlayerPrefs.GetInt(PlayerDiedKey, 0) == 1;


                // After processing the player death, reset the key
                PlayerPrefs.SetInt("PlayerDied", 0); // Reset the key for the next event

                // Set text color based on whether the player is in an event
                elapsedTimeText.color = isInEvent ? eventColor : normalColor;

                // Display the time
                elapsedTimeText.text = elapsedTime;
            }
        }

    }
}
