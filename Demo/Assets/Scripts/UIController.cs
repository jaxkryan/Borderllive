using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private const string ShopInteractedKey = "HasInteractedWithShop";

    public GameObject controlGuidelineImage; // Reference to the control guideline image
    public Canvas settingsCanvas;            // Reference to the settings canvas

    private bool isPaused = false;

    public void StartBtn_Click()
    {
        LevelController.ResetStaticData();
        SceneManager.LoadScene("StoryScene");
    }

    public void ControlBtn_Click()
    {
        // Toggle the visibility of the control guideline image
        controlGuidelineImage.SetActive(!controlGuidelineImage.activeSelf);
    }

    public void RestartBtn_Click()
    {
        LevelController.ResetStaticData();
        if (PlayerPrefs.GetInt(ShopInteractedKey, 0) == 0)
        {
            SceneManager.LoadScene("Room_Start_First");
        }
        else
        {
            SceneManager.LoadScene("Room_Start");
        }
    }

    public void MainMenuBtn_Click()
    {
        Time.timeScale = 1;
        PlayerController.ClearPlayerData();
        SceneManager.LoadScene("MainMenu_Screen");
    }

    public void ExitBtn_Click()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu_Screen");
    }

    void Start()
    {
        settingsCanvas = GetComponent<Canvas>();
        SetupInitialState();

        // Make sure the control guideline image is hidden at the start
        controlGuidelineImage.SetActive(false);
    }

    void SetupInitialState()
    {
        // Activate the settings canvas itself
        settingsCanvas.gameObject.SetActive(true);

        // Deactivate all child objects under the settings canvas (except the control guideline image)
        foreach (Transform child in settingsCanvas.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (controlGuidelineImage.activeSelf)
        {
            // If the control guideline image is up, just turn it off
            controlGuidelineImage.SetActive(false);
        }
        else
        {
            // Toggle the settings menu if the control guideline is not up
            isPaused = !isPaused;

            foreach (Transform child in settingsCanvas.transform)
            {
                // Only toggle settings-related objects, not the control guideline image
                if (child.gameObject != controlGuidelineImage)
                {
                    child.gameObject.SetActive(isPaused);
                }
            }

            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    public void OpenSettings()
    {
        TogglePause();
    }

    public void CloseSettings()
    {
        TogglePause();
    }

    public void ResumeGame()
    {
        CloseSettings();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
