using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ElementalNPC : MonoBehaviour
{
    private Damageable damageable;
    private PlayerInput playerInput;
    public GameObject dialoguePanel;
    public Text dialogueText;
    private int index;
    public GameObject gameCanvas;
    public float wordSpeed;
    public bool playerIsClose;

    // Dialogue lines
    public string[] dialogue;
    public string[] buffChosenDialogue;  // New dialogue for after buff is chosen

    [SerializeField] BuffSelectionUI buffSelectionUI;
    private OwnedPowerups ownedPowerups;
    private bool isChoosing = false;

    [SerializeField] GameObject interactText;
    private bool buffChosen = false;

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

            StartCoroutine(Typing());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerIsClose && !isChoosing)
        {
            if (dialogueText.text == GetCurrentDialogue()[index])
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

    IEnumerator Typing()
    {
        foreach (char letter in GetCurrentDialogue()[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        string[] currentDialogue = GetCurrentDialogue();
        if (index < currentDialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else if (!buffChosen)
        {
            ShowChoices();
        }
        else if (dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }
    }

    // Method to display the choice buttons at the end of the dialogue
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

    // Helper method to determine which dialogue array to use
    private string[] GetCurrentDialogue()
    {
        return buffChosen ? buffChosenDialogue : dialogue;
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
