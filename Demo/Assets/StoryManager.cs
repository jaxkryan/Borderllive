using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to the TextMeshProUGUI UI element
    public Button skipButton; // Reference to the Button UI element
    public Button continueButton; // Reference to the Button UI element to continue to the next scene
    public string[] storyLines; // Array of story lines
    public float textDisplaySpeed = 0.05f; // Speed at which each character appears
    public string nextSceneName = "Room_Start"; // Name of the next scene

    private int currentLineIndex = 0;

    void Start()
    {
        skipButton.onClick.AddListener(SkipStory); // Add listener to the skip button
        continueButton.onClick.AddListener(SkipStory);
        continueButton.gameObject.SetActive(false); // Hide the continue button at the start
        StartCoroutine(DisplayStory());
    }

    IEnumerator DisplayStory()
    {
        while (currentLineIndex < storyLines.Length)
        {
            yield return StartCoroutine(DisplayLine(storyLines[currentLineIndex]));
            currentLineIndex++;
            yield return new WaitForSeconds(0.5f); // Wait before displaying the next line
        }
        continueButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(false);
    }

    IEnumerator DisplayLine(string line)
    {
        foreach (char letter in line.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(textDisplaySpeed);
        }
        storyText.text += "\n"; // Add a new line after each story line is displayed
    }

    void SkipStory()
    {
        StopAllCoroutines();
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
