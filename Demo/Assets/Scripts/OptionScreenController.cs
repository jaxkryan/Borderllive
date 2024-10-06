using UnityEngine;
using UnityEngine.UI;

public class OptionScreenController : MonoBehaviour
{
    private LevelController levelController;
    public Button portalButton1;  // Assign these in the inspector
    public Button portalButton2;
    public Sprite redPortalSprite; // Sprite for battle
    public Sprite bluePortalSprite; // Sprite for event, fun, and shop
    public Text button1Text; // Assign these in the inspector
    public Text button2Text; // Assign these in the inspector

    private void Start()
    {
        // Find the LevelController instance
        levelController = FindObjectOfType<LevelController>();

        if (levelController == null)
        {
            Debug.LogError("LevelController not found in the scene.");
        }
        else
        {
            // Update button display based on selected portals
            UpdateButtonDisplay();
        }
    }

    public void UpdateButtonDisplay()
    {
        // Set button 1 display
        SetButtonDisplay(portalButton1, levelController.SelectedPortal1);
        // Set button 2 display
        SetButtonDisplay(portalButton2, levelController.SelectedPortal2);
    }

    private void SetButtonDisplay(Button button, string portalType)
    {
        string roomType = GetRoomTypeFromName(portalType);  // Convert room name to type

        // Determine the type of room and set the button appearance and text accordingly
        switch (roomType)
        {
            case "battle":
                button.GetComponent<Image>().sprite = redPortalSprite;
                button.GetComponentInChildren<Text>().text = "Battle";
                break;

            case "event":
                button.GetComponent<Image>().sprite = bluePortalSprite;
                button.GetComponentInChildren<Text>().text = "Event";
                break;

            case "fun":
                button.GetComponent<Image>().sprite = bluePortalSprite;
                button.GetComponentInChildren<Text>().text = "Event";
                break;

            case "shop":
                button.GetComponent<Image>().sprite = bluePortalSprite;
                button.GetComponentInChildren<Text>().text = "Shop";
                break;

            default:
                Debug.LogWarning("Unknown portal type: " + roomType);
                break;
        }
    }

    private string GetRoomTypeFromName(string roomName)
    {
        if (roomName.Contains("Battle")) return "battle";
        if (roomName.Contains("Event")) return "event";
        if (roomName.Contains("Fun")) return "event";
        if (roomName.Contains("Shop")) return "shop";
        return null;
    }


    // This method will be called when the player presses the "Q" key (for Portal 1)
    public void OnPortal1Selected()
    {
        if (levelController != null)
        {
            levelController.OnPortal1Selected(); // Load the scene for Portal 1 (first room)
        }
        else
        {
            Debug.LogWarning("LevelController is null. Cannot load Portal 1.");
        }
    }

    // This method will be called when the player presses the "E" key (for Portal 2)
    public void OnPortal2Selected()
    {
        if (levelController != null)
        {
            levelController.OnPortal2Selected(); // Load the scene for Portal 2 (second room)
        }
        else
        {
            Debug.LogWarning("LevelController is null. Cannot load Portal 2.");
        }
    }
}
