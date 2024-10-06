using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;

public class StoryManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public Button skipButton; // Button to skip to the next scene
    public Button continueButton; // Optional, you can use it for functionality
    public LocalizedString[] localizedStoryLines;
    public string tempPassage = "";

    public string previousLine = "";
    public bool[] clearScreenAfterLine; // Boolean array to mark when to clear the screen
    public int[] fontSizeForLine; // Array to store custom font sizes for each line
    public bool[] isItalic;
    public float textDisplaySpeed = 0.05f;
    public string nextSceneName = "Room_Start";
    private int currentLineIndex = 0;
    private bool isDisplaying = false;

    void Start()
    {
        //skipButton.onClick.AddListener(LoadNextScene); // Skip to the next scene
        skipButton.onClick.AddListener(SkipStory);
        continueButton.gameObject.SetActive(false);
        //continueButton.onClick.AddListener(SkipStory);
        StartCoroutine(DisplayStory());
    }

    void Update()
    {
        // Detect mouse click anywhere on the screen
        if (Input.GetMouseButtonDown(0))
        {
            // If the line is currently being displayed, skip to the end of it
            if (isDisplaying)
            {
                StopAllCoroutines();
                StartCoroutine(DisplayFullLine());
            }
            // If all story lines have been displayed, skip to the next scene
            else if (currentLineIndex >= localizedStoryLines.Length)
            {
                SkipStory();
            }
        }
    }


    IEnumerator DisplayStory()
    {
        while (currentLineIndex < localizedStoryLines.Length)
        {
            if (previousLine != "")
            {
                tempPassage += previousLine;
            }
            // Wait for the localized string to be fetched asynchronously
            yield return StartCoroutine(DisplayLine(localizedStoryLines[currentLineIndex], fontSizeForLine[currentLineIndex]));

            // Check if the next line should clear the screen
            if (currentLineIndex < clearScreenAfterLine.Length && clearScreenAfterLine[currentLineIndex + 1])
            {
                yield return StartCoroutine(WaitForClick());
                storyText.text = ""; // Clear the screen
            }

            currentLineIndex++;
            yield return new WaitForSeconds(0.5f); // Small delay before the next line
        }

        //continueButton.gameObject.SetActive(true);
    }

    IEnumerator DisplayLine(LocalizedString localizedLine, int customFontSize)
    {
        // Wait until the localized line is available
        var operation = localizedLine.GetLocalizedStringAsync();
        yield return operation;

        string line = operation.Result;
        previousLine = line;

        // Apply italics if the current line is marked as italic
        if (isItalic != null && currentLineIndex < isItalic.Length && isItalic[currentLineIndex])
        {
            line = "<i>" + line + "</i>"; // Wrap the line in <i> tags for italics
        }

        // Temporarily set a larger font size if customFontSize > 0
        if (customFontSize > 0)
        {
            storyText.fontSize = customFontSize;
        }

        isDisplaying = true; // Mark as displaying text

        // Display each letter one by one
        foreach (char letter in line.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(textDisplaySpeed);
        }

        // After the sentence is displayed, mark isDisplaying as false
        isDisplaying = false;

        // Add a new line after each story line is displayed
        storyText.text += "\n";
    }
    IEnumerator DisplayFullLine()
    {
        // Display the entire current line immediately
        var operation = localizedStoryLines[currentLineIndex].GetLocalizedStringAsync();
        yield return operation;
        string line = operation.Result;
        if (tempPassage != "")
            storyText.text = tempPassage + "\n" + line + "\n"; // Set the full line text
        else storyText.text = line + "\n";
        Debug.Log("Here: " + line);
        tempPassage = "";
        previousLine = "";
        // Move to the next line
        currentLineIndex++;

        // Continue displaying subsequent lines until a line needs to clear the screen
        while (currentLineIndex < localizedStoryLines.Length)
        {
            // Check if the current line requires the screen to be cleared
            if (clearScreenAfterLine[currentLineIndex])
            {
                // If it needs to clear, break out of the loop
                break;
            }

            // Wait for the localized string of the next line
            operation = localizedStoryLines[currentLineIndex].GetLocalizedStringAsync();
            yield return operation;

            // Display the full line without clearing the screen
            line = operation.Result;
            storyText.text += line + "\n"; // Append the line with a newline

            currentLineIndex++; // Move to the next line
        }
        isDisplaying = false;
        yield return StartCoroutine(WaitForClick());

        // Clear the screen before resuming DisplayStory()
        storyText.text = ""; // Clear the screen before resuming
        StartCoroutine(DisplayStory());
    }

    IEnumerator WaitForClick()
    {
        // Wait until the player clicks the screen
        while (!Input.GetMouseButtonDown(0))
        {

            yield return null; // Wait for the next frame
        }
    }

    void SkipStory()
    {
        if (!isDisplaying)
        {
            StopAllCoroutines();
            LoadNextScene();
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
