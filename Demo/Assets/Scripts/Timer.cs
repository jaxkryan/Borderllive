using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Stopwatch stopwatch;
    public Text timerText;

    private const string TimerKey = "SavedTime";  // Key for storing saved time
    private long savedTime = 0;  // Time loaded from PlayerPrefs

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
        // Get the current stopwatch elapsed time + previously saved time
        long elapsedMilliseconds = savedTime + stopwatch.ElapsedMilliseconds;

        // Convert to a more readable format (hours:minutes:seconds:milliseconds)
        int hours = (int)(elapsedMilliseconds / (1000 * 60 * 60)) % 24;
        int minutes = (int)(elapsedMilliseconds / (1000 * 60)) % 60;
        int seconds = (int)(elapsedMilliseconds / 1000) % 60;
        int milliseconds = (int)(elapsedMilliseconds % 1000);

        // Display the time in the console (optional)
        // UnityEngine.Debug.Log($"{hours:D2}:{minutes:D2}:{seconds:D2}:{milliseconds:D3}");

        // Update the UI text if necessary
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
}
