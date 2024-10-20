using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ButtonRedirectTMP : MonoBehaviour
{
    public string targetSceneName; // Name of the scene to load
    public Button button; // Reference to the TMP button component

    private void Start()
    {
        // Add a listener to the button click event
        button.onClick.AddListener(LoadTargetScene);
    }

    private void LoadTargetScene()
    {
        // Load the target scene
        SceneManager.LoadScene(targetSceneName);
    }
}