using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Stopwatch stopwatch;
    public Text timerText;

    private const string TimerKey = "SavedTime";  // Key for storing saved time
    private const string ElapsedTimeKey = "ElapsedTime"; // Key for storing elapsed time as string
    private long savedTime = 0;  // Time loaded from PlayerPrefs
    private bool isPaused = false; // Track if the game is paused

    void Start()
    {
        // Initialize the stopwatch
        stopwatch = new Stopwatch();

        // Load saved time from PlayerPrefs (in milliseconds)
        if (PlayerPrefs.HasKey(TimerKey))
        {
            savedTime = (long)PlayerPrefs.GetFloat(TimerKey, 0);
        }

        stopwatch.Start();  // Start the stopwatch
    }

    void Update()
    {
        if (isPaused) return; // Skip updating when paused

        // Get the current stopwatch elapsed time + previously saved time
        long elapsedMilliseconds = savedTime + stopwatch.ElapsedMilliseconds;

        // Convert to a more readable format (hours:minutes:seconds:milliseconds)
        int hours = (int)(elapsedMilliseconds / (1000 * 60 * 60)) % 24;
        int minutes = (int)(elapsedMilliseconds / (1000 * 60)) % 60;
        int seconds = (int)(elapsedMilliseconds / 1000) % 60;
        int milliseconds = (int)(elapsedMilliseconds % 1000);

        // Update the UI text
        if (timerText != null)
        {
            timerText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
        }
    }

    // Call this method to save the current time
    public void SaveTime()
    {
        // Save the accumulated time (current stopwatch time + previously saved time)
        long elapsedMilliseconds = savedTime + stopwatch.ElapsedMilliseconds;

        // Store in PlayerPrefs as a float
        PlayerPrefs.SetFloat(TimerKey, elapsedMilliseconds);
        PlayerPrefs.Save();

        // Save the elapsed time as a formatted string
        string formattedTime = $"{elapsedMilliseconds / 1000f:F2} s"; // Convert to seconds for display
        PlayerPrefs.SetString(ElapsedTimeKey, formattedTime);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log($"Saved time: {elapsedMilliseconds} ms");
    }

    // Call this method to reset the saved time
    public void ResetTime()
    {
        // Reset stopwatch and saved time
        stopwatch.Reset();
        savedTime = 0;

        // Remove saved time from PlayerPrefs
        PlayerPrefs.DeleteKey(TimerKey);

        UnityEngine.Debug.Log("Time reset");
    }

    // Method triggered when the player dies
    public void OnPlayerDie()
    {
        SaveTime(); // Save the time when the player dies

        // Get the saved elapsed time in milliseconds
        long elapsedMilliseconds = savedTime + stopwatch.ElapsedMilliseconds;

        // Convert milliseconds to seconds (1 second = 1000 milliseconds)
        float elapsedSeconds = elapsedMilliseconds / 1000f;
        if (PlayerPrefs.GetInt("PlayerDied", 0) == 1) // Check the key
        {
            // Calculate currency: 1 second = 2 currency
            int currencyToAdd = (int)(elapsedSeconds * 2); // Multiply seconds by 2

            // Check if PremiumCurrency key exists, if not set it to 0
            if (!PlayerPrefs.HasKey("PremiumCurrency"))
            {
                PlayerPrefs.SetInt("PremiumCurrency", 0);
            }

            // Retrieve the current currency and update it
            int currentCurrency = PlayerPrefs.GetInt("PremiumCurrency");

            PlayerPrefs.SetInt("PremiumCurrency", currentCurrency + currencyToAdd);
            PlayerPrefs.Save(); // Save the changes to PlayerPrefs
            // Optionally, you can log the current currency
            UnityEngine.Debug.Log($"Updated Premium Currency: {currentCurrency + currencyToAdd}");
        }

    }

    // Handle game focus loss (Alt-Tab out) and pause
    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            PauseTimer();
        }
        else
        {
            ResumeTimer();
        }
    }

    // Handle when the application gains or loses focus
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PauseTimer(); // Pause when losing focus
        }
        else
        {
            ResumeTimer(); // Resume when focus is regained
        }
    }

    // Pauses the stopwatch
    private void PauseTimer()
    {
        if (!isPaused)
        {
            stopwatch.Stop();
            isPaused = true;
            UnityEngine.Debug.Log("Timer paused");
        }
    }

    // Resumes the stopwatch
    private void ResumeTimer()
    {
        if (isPaused)
        {
            stopwatch.Start();
            isPaused = false;
            UnityEngine.Debug.Log("Timer resumed");
        }
    }
}
