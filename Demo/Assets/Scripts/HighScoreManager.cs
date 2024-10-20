using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI elapsedTimeText; // Assign this in the Inspector
    public Color normalColor = Color.white; // Color for normal display
    public Color eventColor = Color.red;    // Color for event display
    public Image imageToFlip; // Assign the image in the Inspector

    public Button restartButton; // The restart button from your scene (drag the button here)
    private const string PlayerDiedKey = "PlayerDied"; // Key for tracking player death
    private const string ElapsedTimeKey = "ElapsedTime"; // Key for storing elapsed time as string

    void Start()
    {
        // Check if PlayerDied key is present in PlayerPrefs
        bool isInEvent = PlayerPrefs.GetInt(PlayerDiedKey, 0) == 1;

        // Load the elapsed time string from PlayerPrefs
        if (PlayerPrefs.HasKey(ElapsedTimeKey))
        {
            string elapsedTime = PlayerPrefs.GetString(ElapsedTimeKey);
            if (elapsedTimeText != null)
            {
                // Set text color based on whether the player is in an event
                elapsedTimeText.color = isInEvent ? eventColor : normalColor;

                // Display the time
                elapsedTimeText.text = elapsedTime;

                // Flip the image if the player is in an event
                if (imageToFlip != null)
                {
                    Vector3 currentScale = imageToFlip.transform.localScale;
                    imageToFlip.transform.localScale = isInEvent
                        ? new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z) // Flip horizontally
                        : new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // Reset to normal
                }
            }
        }
        UIController uIController = FindAnyObjectByType<UIController>();
        if (restartButton != null && uIController != null)
        {
            // If the player is in an event, override the Restart button click listener
            if (isInEvent)
            {
                restartButton.onClick.RemoveAllListeners(); // Remove the default onClick listeners
                restartButton.onClick.AddListener(() => uIController.ExtraBtn_Click()); // Add the ExtraBtn_Click listener
            }
            else
            {
                // Add the default Restart listener if not in an event
                restartButton.onClick.AddListener(() => uIController.RestartBtn_Click());
            }
        }

        // After processing the player death, reset the PlayerDied key
        PlayerPrefs.SetInt(PlayerDiedKey, 0); // Reset the key for the next event
    }
}
