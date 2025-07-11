using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private const string doneTutorial = "DoneTutorial";

    public GameObject controlGuidelineImage; // Reference to the control guideline image
    public Canvas settingsCanvas;            // Reference to the settings canvas

    private bool isPaused = false;

    public void StartBtn_Click()
    {
        LevelController.ResetStaticData();
        PlayerPrefs.DeleteKey("ElapsedTime");
        SceneManager.LoadScene("StoryScene");
    }
    public void EndlessBtn_Click()
    {
        // Set the PlayerDied key to true when the button is clicked
        PlayerPrefs.SetInt(PlayerDiedKey, 2); // Set to 1 (true)
        PlayerPrefs.Save(); // Save the changes

        LevelController.ResetStaticData();
        Damageable.defeatedEnemyCount = 0; 
        SceneManager.LoadScene("Endless");
    }
    private const string PlayerDiedKey = "PlayerDied"; // Key for tracking player death

    public void ExtraBtn_Click()
    {
        // Set the PlayerDied key to true when the button is clicked
        PlayerPrefs.SetInt(PlayerDiedKey, 1); // Set to 1 (true)
        PlayerPrefs.Save(); // Save the changes

        LevelController.ResetStaticData();
        SceneManager.LoadScene("BorderDeath_Fun_2");
    }
    public void ControlBtn_Click()
    {
        // Toggle the visibility of the control guideline image
        controlGuidelineImage.SetActive(!controlGuidelineImage.activeSelf);
    }

    public void RestartBtn_Click()
    {
        LevelController.ResetStaticData();
        if (PlayerPrefs.GetInt(doneTutorial) == 0)
        {
            SceneManager.LoadScene("Room_Start_First");
        }
        else
        {
            SceneManager.LoadScene("Room_Start");
        }
        PlayerPrefs.DeleteKey("ElapsedTime");

    }

    public void MainMenuBtn_Click()
    {
        Time.timeScale = 1;
        PlayerController.ClearPlayerData();
        PlayerPrefs.DeleteKey("ElapsedTime");
        SceneManager.LoadScene("MainMenu_Screen");
    }

    public void ShopBtn_Click()
    {
        SceneManager.LoadScene("Shop");
    }

    public void ExitBtn_Click()
    {
        Time.timeScale = 1;
        PlayerPrefs.DeleteKey("ElapsedTime");
        SceneManager.LoadScene("MainMenu_Screen");
    }
    public void BargainBtn_Click()
    {
        SceneManager.LoadScene("Shop");
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
        Timer timer = FindObjectOfType<Timer>();
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

            // Pause or resume the game and timer
            if (isPaused)
            {
                Time.timeScale = 0;
                if(timer != null) timer.PauseTimer();  // Call PauseTimer() from the Timer script
            }
            else
            {
                Time.timeScale = 1;
                if (timer != null) timer.ResumeTimer();  // Call ResumeTimer() from the Timer script
            }
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
