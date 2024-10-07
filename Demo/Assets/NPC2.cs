using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

public class NPC2 : MonoBehaviour
{
    private Damageable damageable;
    private PlayerInput playerInput;
    private CurrencyManager currencyManager;
    public GameObject dialoguePanel;
    public Text dialogueText;
    private int index;
    public GameObject gameCanvas;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject interactableMessage;
    // public PlayerController playerController;
    // Dialogue lines
    public LocalizedString[] dialogue;
    private string resolvedDialogueText; // Add this field to store the resolved text

    // Choice buttons
    public Button choice1Button; // First choice button
    public Button choice2Button; // Second choice button
    public Button choice3Button; // Second choice button
    public LocalizedString[] dialogueOption1; // Array for choice 1
    public LocalizedString[] dialogueOption2; // Array for choice 2
    public LocalizedString[] dialogueOption3; // Array for choice 3

    // Buffs associated with choices
    public Earth_2 choice1Buff; // Buff associated with choice 1
    public Water_2 choice2Buff; // Buff associated with choice 2
    public Fire_2 choice3Buff; // Buff associated with choice 3

    private int numberOfShowChoice = 0;
    private BuffPool buffPool;
    private OwnedPowerups ownedPowerups;
    private bool isChoosing = false; // Flag to check if choices are active

    private void Awake()
    {

        choice1Buff = new Earth_2();
        choice2Buff = new Water_2();
        choice3Buff = new Fire_2();
        // Find the Player GameObject by tag (ensure your Player GameObject is tagged correctly as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Ensure the Player GameObject and PlayerInput component exist
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            damageable = player.GetComponent<Damageable>();
            currencyManager = player.GetComponent<CurrencyManager>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the Player is tagged correctly.");
        }

        // Find the BuffPool and OwnedPowerups
        buffPool = FindObjectOfType<BuffPool>();
        ownedPowerups = FindObjectOfType<OwnedPowerups>();
    }

    void Start()
    {
        dialogueText.text = "";

        // Ensure choice buttons are not visible initially
        choice1Button.gameObject.SetActive(false);
        choice2Button.gameObject.SetActive(false);
        choice3Button.gameObject.SetActive(false);

        // Add listeners to buttons
        choice1Button.onClick.AddListener(() => OnChoiceSelected(1));
        choice2Button.onClick.AddListener(() => OnChoiceSelected(2));
        choice3Button.onClick.AddListener(() => OnChoiceSelected(3));
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isChoosing)
        {
            // Compare the current dialogue text with the resolved dialogue text
            if (dialogueText.text == resolvedDialogueText)
            {
                NextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy && !isChoosing)
        {
            RemoveText();
        }
    }


    public void ShowDialog()
    {
        if (playerIsClose && !isChoosing)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                interactableMessage.SetActive(false);
                playerInput.SwitchCurrentActionMap("Disable");
                gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                if (gameCanvas != null) { gameCanvas.SetActive(false); }

                StartCoroutine(Typing());
            }
        }
        // Check for choice inputs only when choices are active
        if (isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // Press 1
            {
                OnChoiceSelected(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press 2
            {
                OnChoiceSelected(2);
            }
        }
    }
    public void RemoveText()
    {
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        gameCanvas.SetActive(true);

        // Hide choice buttons
        choice1Button.gameObject.SetActive(false);
        choice2Button.gameObject.SetActive(false);

        isChoosing = false; // Reset the choosing flag
    }
    IEnumerator Typing()
    {
        var localizedDialogue = dialogue[index]; // Get the current LocalizedString
        var localizedTextOperation = localizedDialogue.GetLocalizedStringAsync(); // Get the async operation for the string

        yield return localizedTextOperation; // Wait for the operation to complete

        resolvedDialogueText = localizedTextOperation.Result; // Store the resolved text in the variable

        dialogueText.text = ""; // Clear the dialogueText before typing
        foreach (char letter in resolvedDialogueText.ToCharArray()) // Type out the resolved text
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }


    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = ""; // Clear the text
            StartCoroutine(Typing()); // Call the updated Typing coroutine
        }
        else if (numberOfShowChoice == 1)
        {
            RemoveText();
            dialoguePanel.gameObject.SetActive(false);

            //
        }
        else
        {
            numberOfShowChoice++;
            ShowChoices(); // Show choices when reaching the last line
        }
    }


    // Method to display the choice buttons at the end of the dialogue
    private void ShowChoices()
    {
        // Show the choice buttons
        choice1Button.gameObject.SetActive(true);
        choice2Button.gameObject.SetActive(true);
        choice3Button.gameObject.SetActive(true);

        isChoosing = true; // Set the choosing flag to true
    }


    // Method called when a choice is selected
    // private void OnChoiceSelected(int choiceIndex)
    // {
    //     Powerups selectedBuff = null;

    //     if (choiceIndex == 1)
    //     {
    //         Debug.Log("choose 1");
    //         selectedBuff = choice1Buff;
    //     }
    //     else if (choiceIndex == 2)
    //     {
    //         Debug.Log("choose 2");
    //         selectedBuff = choice2Buff;
    //     }

    //     if (selectedBuff != null)
    //     {

    //         ownedPowerups.activePowerups.Add(selectedBuff); // Add selected buff to owned powerups
    //         ownedPowerups.ActivateAPowerup(selectedBuff); // Activate the selected buff
    //         buffPool.RemoveBuff(selectedBuff); // Remove the selected buff from the pool
    //         Debug.Log($"Selected Buff: {selectedBuff.name}");
    //     }


    //     // Remove the dialogue and hide the choices
    //     RemoveText();
    // }
    private void deactiveButton()
    {
        choice1Button.gameObject.SetActive(false);
        choice2Button.gameObject.SetActive(false);
        choice3Button.gameObject.SetActive(false);
    }
    private void OnChoiceSelected(int choiceIndex)
    {
        // Reset the dialogue index to 0
        index = 0;
        deactiveButton();
        // Switch to the selected dialogue array
        if (choiceIndex == 1)
        {
            Debug.Log("Choice 1 selected");
            dialogue = dialogueOption1; // Set to dialogue array for choice 1
            ownedPowerups.activePowerups.Add(choice1Buff);
        }
        else if (choiceIndex == 2)
        {
            Debug.Log("Choice 2 selected");
            dialogue = dialogueOption2; // Set to dialogue array for choice 2
            ownedPowerups.activePowerups.Add(choice2Buff);
        }
        else if (choiceIndex == 3)
        {
            Debug.Log("Choice 3 selected");
            dialogue = dialogueOption3; // Set to dialogue array for choice 3
            ownedPowerups.activePowerups.Add(choice3Buff);
        }

        // After a choice is made, restart the dialogue with the selected option
        RemoveText();  // Clear current text
        ShowDialog();  // Show new dialogue
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
            RemoveText();
        }
    }
}
