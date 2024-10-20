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
        
        // Initialize the pause button's onClick event
        closeButton.onClick.AddListener(ResumeGame);
        tutorial.enabled = true;
        Time.timeScale = 0;
        //Timer.PauseTimer();
        // PauseGameAndShowCanvas();
    }

    // public void PauseGameAndShowCanvas()
    // {
    //     // Pause the game
    //     Time.timeScale = 0;

    //     // Show the pause canvas
        
    // }

    public void ResumeGame()
    {
        Logger.Log("clicked!!!");
        // Unpause the game
        Time.timeScale = 1;
        //Timer.ResumeTimer();
        // Hide the pause canvas
        tutorial.enabled = false;
    }
}
