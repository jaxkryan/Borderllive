using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private const string ShopInteractedKey = "HasInteractedWithShop";

    public void StartBtn_Click()
    {
        LevelController.ResetStaticData();
        SceneManager.LoadScene("StoryScene");
    }

    public void RestartBtn_Click()
    {
        LevelController.ResetStaticData();

        // Check if the player has interacted with a shop before
        if (PlayerPrefs.GetInt(ShopInteractedKey, 0) == 0)
        {
            SceneManager.LoadScene("Room_Start_First");  // Load first-time shop interaction room
        }
        else
        {
            SceneManager.LoadScene("Room_Start");  // Load regular start room
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

    public Canvas settingsCanvas;
    private bool isPaused = false;

    void Start()
    {
        settingsCanvas = GetComponent<Canvas>();
        SetupInitialState();
    }

    void SetupInitialState()
    {
        // Activate the canvas itself
        settingsCanvas.gameObject.SetActive(true);

        // Deactivate all immediate child objects
        foreach (Transform child in transform)
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
        isPaused = !isPaused;

        // Activate/deactivate all child objects based on pause state
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0 : 1;
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
