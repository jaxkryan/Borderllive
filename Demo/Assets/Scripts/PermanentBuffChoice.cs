using UnityEngine;
using UnityEngine.UI;

public class PermanentBuffChoice : MonoBehaviour
{

    [SerializeField] private Toggle permanentBuffToggle;

    private const string ApplyBuffsKey = "ApplyBuffs";

    private void Start()
    {
        // Load saved toggle state (default is unchecked, i.e., false)
        bool savedToggleState = PlayerPrefs.GetInt(ApplyBuffsKey, 0) == 1;
        permanentBuffToggle.isOn = savedToggleState;

        // Add listener to save the state when the toggle is changed
        permanentBuffToggle.onValueChanged.AddListener(SaveToggleState);
    }

    private void SaveToggleState(bool isOn)
    {
        // Save the toggle state to PlayerPrefs
        PlayerPrefs.SetInt(ApplyBuffsKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Call this method when the player starts the game (e.g., Play button)
    public void StartGame()
    {
        // Load the game scene or gameplay logic
        // Example: SceneManager.LoadScene("GameScene");
    }

}
