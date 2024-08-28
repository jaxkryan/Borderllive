using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void StartBtn_Click()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void RestartBtn_Click()
    {
        SceneManager.LoadScene("Room_Start");
    }

    public void MainMenuBtn_Click()
    {
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