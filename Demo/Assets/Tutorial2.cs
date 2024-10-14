using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour
{
    public GameObject canvas;
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
