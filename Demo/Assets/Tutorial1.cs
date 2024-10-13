using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    public GameObject exchanger;
    private const string isFirstPlayKey = "IsFirstPlay";
    private const int isFirstPlayValue = 1; // 1 = not first play, 0 = first play
    public GameObject canvas;

    void Update()
    {
        if (!PlayerPrefs.HasKey(isFirstPlayKey) || PlayerPrefs.GetInt(isFirstPlayKey) == 0)
        {
            Debug.Log("not first play");
            // First play, set the flag
            PlayerPrefs.SetInt(isFirstPlayKey, isFirstPlayValue);
            // Hide the game object
            gameObject.SetActive(false);
        }
        else
        {Debug.Log("yes first play");
            // Not the first play, show the game object
            gameObject.SetActive(true);
        }
    }

        void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the canvas
        if (collision.gameObject.CompareTag("Player"))
        {
            // Show the canvas
            canvas.SetActive(true);
            // Also, set the Tutorial1 script's game object to active
            //tutorialScript.gameObject.SetActive(true);
        }

        
    }

        void OnTriggerExit2D(Collider2D collision)
    {

            canvas.SetActive(false);
    }


}
