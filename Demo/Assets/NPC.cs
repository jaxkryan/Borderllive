using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject dialoguePanel;
    public Text dialogueText;
    private int index;
    private Coroutine co;
    public GameObject gameCanvas;
    public GameObject nextButton;
    public float wordSpeed;
    public bool playerIsClose;

    // EnemySpawnPoint reference
    public string[] dialogue;

    private void Awake()
    {
        // Find the Player GameObject by tag (ensure your Player GameObject is tagged correctly as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Ensure the Player GameObject and PlayerInput component exist
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the Player is tagged correctly.");
        }
    }

    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
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
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
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
            RemoveText();
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
            RemoveText();
        }
    }
}