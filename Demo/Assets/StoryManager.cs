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

    private int currentLineIndex = 0;
    private bool isDisplaying = false;

    // Key for shop interaction status
    private const string doneTutorial = "DoneTutorial";

    void Start()
    {
        skipButton.onClick.AddListener(SkipStory);
        continueButton.gameObject.SetActive(false);
        StartCoroutine(DisplayStory());
    }

    void Update()
    {
        // Detect mouse click anywhere on the screen
        if (Input.GetMouseButtonDown(0))
        {
            if (isDisplaying)
            {
                StopAllCoroutines();
                StartCoroutine(DisplayFullLine());
            }
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
            yield return StartCoroutine(DisplayLine(localizedStoryLines[currentLineIndex], fontSizeForLine[currentLineIndex]));

            if (currentLineIndex < clearScreenAfterLine.Length && clearScreenAfterLine[currentLineIndex + 1])
            {
                yield return StartCoroutine(WaitForClick());
                storyText.text = "";  // Clear the screen
            }

            currentLineIndex++;
            yield return new WaitForSeconds(0.5f);  // Small delay before the next line
        }

        // After the story ends, load the next scene
        LoadNextScene();
    }

    IEnumerator DisplayLine(LocalizedString localizedLine, int customFontSize)
    {
        var operation = localizedLine.GetLocalizedStringAsync();
        yield return operation;

        string line = operation.Result;
        previousLine = line;

        if (isItalic != null && currentLineIndex < isItalic.Length && isItalic[currentLineIndex])
        {
            line = "<i>" + line + "</i>";
        }

        if (customFontSize > 0)
        {
            storyText.fontSize = customFontSize;
        }

        isDisplaying = true;

        foreach (char letter in line.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(textDisplaySpeed);
        }

        isDisplaying = false;
        storyText.text += "\n";
    }

    IEnumerator DisplayFullLine()
    {
        var operation = localizedStoryLines[currentLineIndex].GetLocalizedStringAsync();
        yield return operation;
        string line = operation.Result;
                if (isItalic != null && currentLineIndex < isItalic.Length && isItalic[currentLineIndex])
        {
            line = "<i>" + line + "</i>";
        }
        storyText.text = tempPassage + "\n" + line + "\n";
        tempPassage = "";
        previousLine = "";
        currentLineIndex++;

        while (currentLineIndex < localizedStoryLines.Length)
        {
            if (clearScreenAfterLine[currentLineIndex])
            {
                break;
            }
        if (isItalic != null && currentLineIndex < isItalic.Length && isItalic[currentLineIndex])
        {
            line = "<i>" + line + "</i>";
        }
            operation = localizedStoryLines[currentLineIndex].GetLocalizedStringAsync();
            yield return operation;

            line = operation.Result;
            storyText.text += line + "\n";
            currentLineIndex++;
        }
        isDisplaying = false;
        yield return StartCoroutine(WaitForClick());
        storyText.text = "";
        StartCoroutine(DisplayStory());
    }

    IEnumerator WaitForClick()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
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
        // Check if the player has interacted with a shop before
        if (PlayerPrefs.GetInt(doneTutorial, 0) == 0)
        {
            SceneManager.LoadScene("Room_Start_First");  // Load first-time shop interaction room
        }
        else
        {
            Logger.Log("room start go first smh");
            SceneManager.LoadScene("Room_Start");  // Load regular start room
        }
    }
}
