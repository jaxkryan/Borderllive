using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public List<string[]> dialogueSets;  // List of dialogue sets
    private string[] currentDialogue;
    private int index;
    private Coroutine co;
    public GameObject gameCanvas;
    public GameObject nextButton;
    public float wordSpeed;
    public bool playerIsClose;

    // EnemySpawnPoint reference
    public GameObject[] enemySpawnPoint;

    private bool hasShownInitialDialogue = false;  // Flag to track initial dialogue display

    // Start is called before the first frame update
    void Start()
    {
        dialogueSets = new List<string[]>();  // Initialize the list
        dialogueSets.Add(new string[] { "Here is Borderdeath", "And you are a lost soul", "Just go ahead", "And your next life will be shown", "Ok joke aside, you are dead, and this is borderdeath - a land between life and death.", "You must fight for yourself to find a way to your next life, or else you will stuck here forever.", "I will give you these thing to help you fight for your second life.", "Good luck!" });
        dialogueSets.Add(new string[] { "Quick, go ahead", "And find the truth yourself" });
        dialogueSets.Add(new string[] { "How many times i meet you?", "Just wanna say good luck on your journey to ur next life" });
        dialogueSets.Add(new string[] { "Sometime just sit down and chill", "U have plenty of time here anyway" });
        dialogueSets.Add(new string[] { "Next life will be better", "Trust me bro" });
        dialogueSets.Add(new string[] { "Maybe you can carry me to your next life", "Of course if only you can lift me up first" });
        dialogueSets.Add(new string[] { "Do you need me to delete your history browser?", "If yes, swing your sword" });

        enemySpawnPoint = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
        if (enemySpawnPoint == null)
        {
            Debug.LogError("EnemySpawnPoint not found! Make sure it's tagged correctly.");
        }

        // Check if initial dialogue has been shown
        if (!PlayerPrefs.HasKey("InitialDialogueShown"))
        {
            PlayerPrefs.SetInt("InitialDialogueShown", 0);
        }
        else
        {
            hasShownInitialDialogue = PlayerPrefs.GetInt("InitialDialogueShown") == 1;
        }
    }

    IEnumerator Typing()
    {
        Debug.Log("previous: " + currentDialogue[index]);

        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(wordSpeed * Time.timeScale);
        }

        Debug.Log("current: " + dialogueText.text);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel != null && dialoguePanel.activeInHierarchy)
            {
                EndDialogue();
            }
            else
            {
                StartDialogue();
            }
        }

        // Check if dialogueText and currentDialogue are not null before accessing them
        if (dialogueText != null && currentDialogue != null && index < currentDialogue.Length)
        {
            if (dialogueText.text == currentDialogue[index])
            {
                nextButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(false);
            }
        }
    }


    public void NextLine()
    {
        nextButton.SetActive(false);
        if (index < currentDialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            co = StartCoroutine(Typing());
        }
        else
        {
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        // Disable EnemySpawnPoint
        if (gameCanvas != null) { gameCanvas.SetActive(false); }
        if (enemySpawnPoint != null)
        {
            foreach (GameObject e in enemySpawnPoint)
            {
                e.SetActive(false);
            }
        }

        // Select dialogue set based on whether initial dialogue has been shown
        if (!hasShownInitialDialogue)
        {
            currentDialogue = dialogueSets[0];  // Show initial dialogue set
            PlayerPrefs.SetInt("InitialDialogueShown", 1);  // Mark initial dialogue as shown
        }
        else
        {
            SelectRandomDialogue();  // Show random dialogue set
        }

        dialoguePanel.SetActive(true);
        StartCoroutine(Typing());

    }

    private void EndDialogue()
    {
        // Enable EnemySpawnPoint if they exist and are not destroyed
        if (gameCanvas != null) gameCanvas.SetActive(true);
        if (enemySpawnPoint != null)
        {
            foreach (GameObject e in enemySpawnPoint)
            {
                // Check if the GameObject reference is not null and is not destroyed
                if (e != null)
                {
                    e.SetActive(true);
                }
            }
        }

        if (dialogueText != null && dialogueText.gameObject.activeInHierarchy)
        {
            dialogueText.text = "";
        }

        index = 0;
        if (dialoguePanel != null && dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(false);
        }
    }


    private void SelectRandomDialogue()
    {
        if (dialogueSets.Count > 1)  // Make sure there are multiple dialogue sets available
        {
            int randomIndex = Random.Range(1, dialogueSets.Count);  // Start from index 1 to skip initial dialogue set
            currentDialogue = dialogueSets[randomIndex];
            index = 0;  // Reset index for new dialogue
        }
        else
        {
            Debug.LogError("Not enough dialogue sets available.");
        }
    }

    public void zeroText()
    {
        // Reset dialogue UI
        if (dialogueText != null && dialogueText.gameObject.activeInHierarchy)
        {
            dialogueText.text = "";
        }

        index = 0;
        if (dialoguePanel != null && dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(false);
        }

        gameCanvas.SetActive(true);
        // Check and reset enemySpawnPoint
        if (enemySpawnPoint != null)
        {
            for (int i = 0; i < enemySpawnPoint.Length; i++)
            {
                if (enemySpawnPoint[i] != null)
                {
                    enemySpawnPoint[i].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            EndDialogue();
        }
    }
}