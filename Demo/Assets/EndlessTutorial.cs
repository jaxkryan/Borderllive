using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessTutorial : MonoBehaviour
{
    public Button closeButton; // Assign your pause button in the inspector
    public Canvas tutorial; // Assign your pause canvas in the inspector
    public Timer Timer;
    private void Start()
    {
        Timer.PauseTimer();
        // Initialize the pause button's onClick event
        closeButton.onClick.AddListener(ResumeGame);

        PauseGameAndShowCanvas();
    }

    public void PauseGameAndShowCanvas()
    {
        // Pause the game
        Time.timeScale = 0;

        // Show the pause canvas
        tutorial.enabled = true;
    }

    public void ResumeGame()
    {
        // Unpause the game
        Time.timeScale = 1;
        Timer.ResumeTimer();
        // Hide the pause canvas
        tutorial.enabled = false;
    }
}
