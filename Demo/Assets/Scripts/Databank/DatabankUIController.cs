using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabankUIController : MonoBehaviour
{

    public GameObject collectionPanel;  // The whole panel that contains the items and powerups
    public Databank databank;           // Reference to the Databank script

    private void Start()
    {
        // Ensure the collection panel is hidden on start
        collectionPanel.SetActive(false);
    }

    // Function to show the collection panel when the book icon is clicked
    public void ShowCollectionPanel()
    {
        collectionPanel.SetActive(true);      // Show the panel
        ShowPowerups();                       // Display powerups by default
    }

    // Function to hide the collection panel
    public void HideCollectionPanel()
    {
        collectionPanel.SetActive(false);     // Hide the panel
    }

    // Function to show items in the collection
    public void ShowItems()
    {
        databank.RefreshItemDisplay();  // Use the Databank script to display items
    }

    // Function to show powerups in the collection
    public void ShowPowerups()
    {
        databank.RefreshPowerupDisplay();  // Use the Databank script to display powerups
    }

}
