using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopNPC : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject shopPanel;  // Reference to the shop panel or ShopManager
    private ShopManager shopManager;  // Reference to ShopManager
     public GameObject gameCanvas;
    public bool playerIsClose;  // Flag to check if the player is near the NPC

    private void Awake()
    {
        // Find the Player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
       
        // Ensure the Player exists
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the Player is tagged correctly.");
        }

        // Find the ShopManager (assuming there is only one ShopManager in the scene)
        shopManager = FindObjectOfType<ShopManager>();
    }

    void Update()
    {

    }

    // Method to open the shop menu
    public void OpenShop()
    {
        if (shopPanel != null && shopManager != null)
        {
            shopPanel.SetActive(true);
            // shopManager.DisplayItems();  // Populate the shop with items
            playerInput.SwitchCurrentActionMap("Disable");  // Disable player input while the shop is open
            gameCanvas.SetActive(false);
        }
    }

    // Optional: Method to close the shop menu
    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");  // Enable player input after closing the shop
        }
         if (gameCanvas != null) { gameCanvas.SetActive(true); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            CloseShop();  // Close the shop if the player moves away
        }
    }
}
