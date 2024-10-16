using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ElementalNPC : MonoBehaviour
{
    private Damageable damageable;
    private PlayerInput playerInput;
    public GameObject dialoguePanel;
    public Text dialogueText;
    private int index;
    private GameObject gameCanvas;
    public float wordSpeed;

    // Localized dialogue lines
    public LocalizedString[] npcDialogue;     // NPC's localized dialogue lines
    public LocalizedString[] playerDialogue;   // Player's localized response lines
    public LocalizedString[] buffChosenDialogue; // Localized dialogue for after buff is chosen
    public LocalizedString[] commentorDialogue; // Commentor's localized dialogue lines
    [SerializeField] List<int> dialogueTurns = new List<int>();
    [SerializeField] BuffSelectionUI buffSelectionUI;
    private OwnedPowerups ownedPowerups;
    private bool isChoosing = false;
    private bool buffChosen = false;

    [SerializeField] GameObject interactText;

    private string resolvedDialogueText; // Store the resolved text for typing

    private void Awake()
    {
        buffChosen = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            damageable = player.GetComponent<Damageable>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the Player is tagged correctly.");
        }

        ownedPowerups = FindObjectOfType<OwnedPowerups>();
    }

    void Start()
    {
        dialogueText.text = "";
    }

    public void ShowDialog()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            interactText.SetActive(false);
            dialoguePanel.SetActive(true);
            playerInput.SwitchCurrentActionMap("Disable");

            gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
            if (gameCanvas != null) { gameCanvas.SetActive(false); }

            index = 0;
            StartCoroutine(TypingDialogue()); // Start dialogue according to turn order
        }
    }

    void Update()
    {
        // Advance dialogue when clicking or pressing a button
        if (Input.GetMouseButtonDown(0) && !isChoosing)
        {
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
        isChoosing = false;
    }

    // Typing out the dialogue for either NPC, Player, or Commentor based on the current turn
    IEnumerator TypingDialogue()
    {
        var currentDialogue = GetCurrentDialogue()[index]; // Get the current LocalizedString
        var localizedTextOperation = currentDialogue.GetLocalizedStringAsync(); // Get the async operation for the localized string

        yield return localizedTextOperation; // Wait for the operation to complete

        resolvedDialogueText = localizedTextOperation.Result; // Store the resolved text

        dialogueText.text = ""; // Clear the dialogueText before typing
        foreach (char letter in resolvedDialogueText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (!buffChosen)
        {
            index++;
            dialogueText.text = "";

            if (index < GetCurrentDialogue().Length)
            {
                StartCoroutine(TypingDialogue()); // Continue dialogue based on turns
            }
            else
            {
                ShowChoices(); // End of dialogue, show buff choices
            }
        }
        else
        {
            if (dialoguePanel.activeInHierarchy)
            {
                RemoveText();
            }
        }
    }

    // Show Buff Selection UI
    private void ShowChoices()
    {
        buffSelectionUI.ShowBuffChoices();
        isChoosing = true;
        buffChosen = true;
        if (dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }
    }

    // Determine whether the NPC, Player, or Commentor speaks based on the index
    private LocalizedString[] GetCurrentDialogue()
    {
        if (buffChosen)
        {
            return buffChosenDialogue;
        }

        // Check whose turn it is based on dialogueTurns list
        int? turn = dialogueTurns[index];

        if (turn == 1) // NPC turn
        {
            return npcDialogue;
        }
        else if (turn == 2) // Player turn
        {
            return playerDialogue;
        }
        else if (turn == 3) // Commentor turn
        {
            return commentorDialogue;
        }
        else
        {
            Debug.LogWarning("Invalid dialogue turn detected: " + turn);
            return npcDialogue; // Fallback to NPC dialogue if turn is invalid
        }
    }
}
