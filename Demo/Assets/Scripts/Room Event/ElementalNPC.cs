using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ElementalNPC : MonoBehaviour
{
    private Damageable damageable;
    private PlayerInput playerInput;
    public GameObject dialoguePanel; 
    public TextMeshProUGUI dialogueText; // Replace Text with TextMeshProUGUI
    private int dialogueTurnIndex; // Used to track whose turn it is
    private int npcDialogueIndex;
    private int playerDialogueIndex;
    private int commentorDialogueIndex;
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

    // NPC images for hiding and showing
    public List<Image> npcImages;  // List of NPC images that hide when NPC is done talking
    public List<Image> npcImageFades;  // List of NPC images that stay visible after NPC talks for the first time
    private bool npcFirstTimeTalking = true; // Track if it's the NPC's first time talking

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

        // Initially hide the NPC images
        foreach (var img in npcImages)
        {
            img.gameObject.SetActive(false);
        }

        // Initially hide the NPC fade images as well
        foreach (var img in npcImageFades)
        {
            img.gameObject.SetActive(false);
        }
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

            // Reset indices when dialogue starts
            dialogueTurnIndex = 0;
            npcDialogueIndex = 0;
            playerDialogueIndex = 0;
            commentorDialogueIndex = 0;

            StartCoroutine(TypingDialogue());
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
        dialogueTurnIndex = 0;
        dialoguePanel.SetActive(false);
        gameCanvas.SetActive(true);
        isChoosing = false;

        // Hide the NPC images when the dialogue is done
        foreach (var img in npcImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    // Typing out the dialogue for either NPC, Player, or Commentor based on the current turn
    IEnumerator TypingDialogue()
    {
        var currentDialogue = GetCurrentDialogue(); // Get the correct LocalizedString based on turns
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

    // Ensure to reset npcDialogueIndex when starting buffChosenDialogue
    public void NextLine()
    {
        if (!buffChosen)
        {
            dialogueTurnIndex++;
            dialogueText.text = "";

            if (dialogueTurnIndex < dialogueTurns.Count)
            {
                StartCoroutine(TypingDialogue()); // Continue normal dialogue based on turns
            }
            else
            {
                ShowChoices(); // End of normal dialogue, show buff choices
            }
        }
        else
        {
            // Ensure to use separate npcDialogueIndex for buffChosenDialogue
            if (npcDialogueIndex >= buffChosenDialogue.Length)
            {
                RemoveText(); // End dialogue if no more buffChosenDialogue is available
                return; // Exit to avoid further execution
            }

            StartCoroutine(ShowBuffChosenDialogue()); // Show the next buff chosen dialogue
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

    private IEnumerator ShowBuffChosenDialogue()
    {
        // Hide npcImages and show npcImageFades when displaying buffChosenDialogue
        foreach (var img in npcImages)
        {
            img.gameObject.SetActive(true); // Hide regular NPC images
        }

        foreach (var img in npcImageFades)
        {
            img.gameObject.SetActive(false); // Show faded NPC images
        }

        // Use the npcDialogueIndex for buffChosenDialogue
        var localizedBuffDialogue = buffChosenDialogue[npcDialogueIndex].GetLocalizedStringAsync();
        yield return localizedBuffDialogue;

        resolvedDialogueText = localizedBuffDialogue.Result;
        dialogueText.text = "";

        foreach (char letter in resolvedDialogueText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        npcDialogueIndex++; // Move to next buffChosenDialogue
    }


    // Determine whose dialogue to play and use the respective index
    private LocalizedString GetCurrentDialogue()
    {
        int turn = dialogueTurns[dialogueTurnIndex];

        if (turn == 1) // NPC turn
        {
            if (npcFirstTimeTalking)
            {
                foreach (var img in npcImageFades)
                {
                    img.gameObject.SetActive(true); // Show the NPC fade images
                }
                npcFirstTimeTalking = false; // Mark that NPC has talked
            }
            foreach (var img in npcImages)
            {
                img.gameObject.SetActive(true); // Show NPC images temporarily
            }
            return npcDialogue[npcDialogueIndex++];
        }

        foreach (var img in npcImages)
        {
            img.gameObject.SetActive(false); // Hide NPC images when it's not their turn
        }

        if (turn == 2) // Player turn
        {
            return playerDialogue[playerDialogueIndex++];
        }
        else if (turn == 3) // Commentor turn
        {
            return commentorDialogue[commentorDialogueIndex++];
        }
        else
        {
            Debug.LogWarning("Invalid dialogue turn detected: " + turn);
            return npcDialogue[0]; // Fallback to NPC dialogue if turn is invalid
        }
    }
}
