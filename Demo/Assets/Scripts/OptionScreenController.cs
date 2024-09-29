using UnityEngine;

public class OptionScreenController : MonoBehaviour
{
    private LevelController levelController;

    private void Start()
    {
        // Find the LevelController instance
        levelController = FindObjectOfType<LevelController>();

        if (levelController == null)
        {
            Debug.LogError("LevelController not found in the scene.");
        }
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
