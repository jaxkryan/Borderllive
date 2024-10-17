using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI elapsedTimeText; // Assign this in the Inspector

    void Start()
    {
        // Load the elapsed time string from PlayerPrefs
        if (PlayerPrefs.HasKey("ElapsedTime"))
        {
            string elapsedTime = PlayerPrefs.GetString("ElapsedTime");
            if (elapsedTimeText != null)
            {
                elapsedTimeText.text = "Your Time: " + elapsedTime; // Display the time
            }
        }

    }

}
