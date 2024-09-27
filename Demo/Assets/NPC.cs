using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
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

    // Choice buttons
    public Button choice1Button; // First choice button
    public Button choice2Button; // Second choice button

    // Buffs associated with choices
    public Metal_1 choice1Buff; // Buff associated with choice 1
    public Earth_1 choice2Buff; // Buff associated with choice 2

    private BuffPool buffPool;
    private OwnedPowerups ownedPowerups;
    private bool isChoosing = false; // Flag to check if choices are active

    private void Awake()
    {

        choice1Buff = new Metal_1();
        choice2Buff = new Earth_1();
        // Find the Player GameObject by tag (ensure your Player GameObject is tagged correctly as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Ensure the Player GameObject and PlayerInput component exist
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            damageable = player.GetComponent<Damageable>();
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

        // Add listeners to buttons
        choice1Button.onClick.AddListener(() => OnChoiceSelected(1));
        choice2Button.onClick.AddListener(() => OnChoiceSelected(2));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !isChoosing)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                playerInput.SwitchCurrentActionMap("Disable");
                gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
                if (gameCanvas != null) { gameCanvas.SetActive(false); }

                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy && !isChoosing)
        {
            RemoveText();
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
        foreach (char letter in dialogue[index].ToCharArray())
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
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ShowChoices(); // Show choices when reaching the last line
        }
    }

    // Method to display the choice buttons at the end of the dialogue
    private void ShowChoices()
    {
        // Show the choice buttons
        choice1Button.gameObject.SetActive(true);
        choice2Button.gameObject.SetActive(true);

        isChoosing = true; // Set the choosing flag to true

        // // Set button texts to show buffs' names (optional)
        // choice1Button.GetComponentInChildren<Text>().text = choice1Buff.name;
        // choice2Button.GetComponentInChildren<Text>().text = choice2Buff.name;
    }

    // Method called when a choice is selected
    private void OnChoiceSelected(int choiceIndex)
    {
        Powerups selectedBuff = null;

        if (choiceIndex == 1)
        {
            Debug.Log("choose 1");
            selectedBuff = choice1Buff;
        }
        else if (choiceIndex == 2)
        {
            Debug.Log("choose 2");
            selectedBuff = choice2Buff;
        }

        if (selectedBuff != null)
        {

            ownedPowerups.activePowerups.Add(selectedBuff); // Add selected buff to owned powerups
            ownedPowerups.ActivateAPowerup(selectedBuff); // Activate the selected buff
            buffPool.RemoveBuff(selectedBuff); // Remove the selected buff from the pool
            Debug.Log($"Selected Buff: {selectedBuff.name}");
        }


        // Remove the dialogue and hide the choices
        RemoveText();
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
