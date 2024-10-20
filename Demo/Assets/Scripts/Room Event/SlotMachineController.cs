using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotMachineController : MonoBehaviour
{
    public Reel[] reels; // Array to hold all the reel components
    public Text rewardText; // UI text to display rewards or messages
    public Text spendingText; // UI text to display rewards or messages
    public GameObject slotMachineUI; // Container for the slot machine UI (reels, button, etc.)
    public GameObject HUD; // The player's HUD (to be hidden when playing)
    private CurrencyManager currency; // Player's current coin count
    public Text currencyText; // UI Text to display current currency amount
    private PlayerInput playerInput;
    private bool isSpinning = false; // Flag to prevent multiple spins
    private int spinCount = 0; // Counter for the number of spins
    public const int maxSpins = 10; // Maximum number of spins allowed

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        HUD = GameObject.FindGameObjectWithTag("GameCanvas");
        currency = FindObjectOfType<CurrencyManager>();
        // Ensure the Player exists
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not found. Ensure the Player is tagged correctly.");
        }

        // Initially hide the slot machine UI
        slotMachineUI.SetActive(false);

        // Update the currency text on game start
        UpdateCurrencyText();
        spendingText.text = "Free";
    }

    public void ShowSlotMachine()
    {
        playerInput.SwitchCurrentActionMap("Disable");  // Disable player input while the machine is open
        // Hide HUD and show the slot machine UI
        HUD.SetActive(false);
        slotMachineUI.SetActive(true);
        rewardText.text = ""; // Clear any previous reward message
    }

    public void SpinReels()
    {
        rewardText.text = "";
        int spendAmount = reels.Length * 10 * spinCount;
        // Prevent multiple spins if a spin is already in progress
        if (isSpinning)
        {
            Debug.Log("Reels are already spinning. Please wait.");
            return;
        }

        // Check if player has enough currency to spin
        if (currency.currentAmount < spendAmount)
        {
            rewardText.text = "Not enough souls";
        }
        else if (spinCount >= maxSpins) // Check if the maximum spins have been reached
        {
            rewardText.text = "No more spins allowed!";
            DisableSlotMachine(); // Disable the slot machine
        }
        else
        {
            // Deduct currency and start the spin
            currency.SpendCurrency(spendAmount);
            UpdateCurrencyText();

            // Increment spin count
            spinCount++;

            // Set isSpinning to true to prevent additional spins
            isSpinning = true;

            // Spin each reel
            foreach (Reel reel in reels)
            {
                reel.Spin();
            }
            spendAmount = reels.Length * 10 * spinCount;
            spendingText.text = "Spend " + spendAmount.ToString() + " souls?";
            // Check the result after a short delay (adjust timing based on spin animation)
            Invoke("CheckResult", 5f);
        }
    }

    void CheckResult()
    {
        Sprite firstReelSymbol = reels[0].reelImage.sprite;
        bool isWin = true;
        bool isJackpot = false; // Flag to check for the special jackpot
        int reward = 70;
        reward += reels.Length * 50;
        // Check if all reels have the same symbol
        for (int i = 1; i < reels.Length; i++)
        {
            if (reels[i].reelImage.sprite != firstReelSymbol)
            {
                isWin = false;
                break;
            }
        }

        if (firstReelSymbol.name == "JackPot" && isWin)
        {
            isJackpot = true;
        }

        // Display the result and reward the player
        if (isJackpot)
        {
            reward *= 4;
            rewardText.text = "Jackpot! +" + reward + " Souls!";
            RewardPlayer(reward);
        }
        else if (isWin)
        {
            rewardText.text = "Win! +" + reward + " Souls!";
            RewardPlayer(reward);
        }
        else
        {
            rewardText.text = "Try Again!";
        }

        // Reset isSpinning to false to allow another spin
        isSpinning = false;
    }

    void RewardPlayer(int rewardAmount)
    {
        currency.AddCurrency(rewardAmount);
        Debug.Log("Player awarded " + rewardAmount + " souls. Total souls: " + currency.currentAmount);

        // Update the currency UI text after rewarding the player
        UpdateCurrencyText();
    }

    public void HideSlotMachineAndShowHUD()
    {
        if (isSpinning)
        {

        }
        else

        {
            playerInput.SwitchCurrentActionMap("Player");  // Enable player input after closing the slot machine

            // Hide the slot machine and show the HUD
            slotMachineUI.SetActive(false);
            HUD.SetActive(true);
        }
    }

    // Method to update the currency text on the screen
    void UpdateCurrencyText()
    {
        currencyText.text = ": " + currency.currentAmount.ToString();
    }

    // Method to disable the slot machine
    void DisableSlotMachine()
    {
        playerInput.SwitchCurrentActionMap("Player");  // Enable player input after closing the slot 
        slotMachineUI.SetActive(false);
        HUD.SetActive(true); // Optionally, you can show the HUD again or keep it hidden
        Debug.Log("The slot machine is now closed.");
    }
}
