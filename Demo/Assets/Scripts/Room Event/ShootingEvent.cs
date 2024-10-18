using UnityEngine;
using UnityEngine.UI; // For working with UI elements
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;

public class ShootingEvent : MonoBehaviour
{
    public List<GameObject> leftShooters; // List of left shooters
    public List<GameObject> rightShooters; // List of right shooters
    public List<GameObject> verticalShooters;   // List of vertical shooters
    public GameObject portal;                   // Portal GameObject that will appear after waves are completed
    public TMP_Text messageText;                // UI Text to display messages
    public int totalWaves = 5;                  // Total number of waves before the portal appears
    public float portalDelay = 3f;              // Delay before portal appears after final wave
    public GameObject damageableTarget;         // Reference to the Damageable GameObject

    private float timer;
    private int currentWave = 0;                // Track the current wave number
    private bool firstWaveStarted = false;      // Check if the first wave has started
    public float firstWaveDelay = 8f;           // Delay before the first wave

    private float projectileSpeedIncrement = 0f; // Cumulative increase in projectile speed

    void Start()
    {
        buffSelectionUI = FindObjectOfType<BuffSelectionUI>(); // Find the BuffSelectionUI
        currencyManager = FindAnyObjectByType<CurrencyManager>();
        timer = firstWaveDelay;                 // Start with a delay for the first wave
        portal.SetActive(false);                // Deactivate the portal at the beginning
    }

    void Update()
    {
        // Check if there are waves remaining
        if (currentWave < totalWaves)
        {
            timer -= Time.deltaTime;

            // Handle the first wave separately with the delay
            if (!firstWaveStarted && timer <= 0f)
            {
                firstWaveStarted = true;
                FireRandomPattern();
                currentWave++;

                timer = 3f; // Set timer for next wave to normal interval
                if (messageText != null)
                {
                    messageText.gameObject.SetActive(false); // Deactivate the messageText after the first wave starts
                }
            }

            // Handle subsequent waves normally after the first wave
            if (firstWaveStarted && timer <= 0f)
            {
                FireRandomPattern();
                timer = Random.Range(2, 5); // Normal interval between waves
                currentWave++;

                // Increase projectile speed after every 5 rounds
                if (currentWave % 5 == 0)
                {
                    projectileSpeedIncrement += 1f; // Increase speed by 1f after every 5 waves
                    HealDamageable();        // Heal the damageable target
                }

                // If the last wave has been reached, wait and then activate the portal
                if (currentWave >= totalWaves)
                {
                    StartCoroutine(ActivatePortalWithDelay());
                }
            }
        }
    }

    void FireRandomPattern()
    {
        // Define patterns and their associated weights
        Dictionary<string, int> patterns = new Dictionary<string, int>
    {
        { "1L 1R", 10 },
        { "1L 2V", 20 },
        { "1R 2V", 20 },
        { "3V", 15 },
        { "2V", 15 },
        { "1L 1R 1V", 10 },
        { "1L 1R 2V", 10 }
    };

        string selectedPattern = SelectPattern(patterns);

        // Parse the selected pattern and fire projectiles accordingly
        string[] commands = selectedPattern.Split(' ');

        foreach (string command in commands)
        {
            if (command == "1L")
            {
                FireLeft(1);
            }
            else if (command == "1R")
            {
                FireRight(1);
            }
            else if (command.StartsWith("2V"))
            {
                int count = int.Parse(command[0].ToString());
                FireVertical(count);
            }
            else if (command.StartsWith("3V"))
            {
                int count = int.Parse(command[0].ToString());
                FireVertical(count);
            }
        }
    }

    // Method to select a pattern based on weighted probabilities
    private string SelectPattern(Dictionary<string, int> patterns)
    {
        int totalWeight = 0;

        // Calculate the total weight
        foreach (var weight in patterns.Values)
        {
            totalWeight += weight;
        }

        // Select a random value
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        // Find which pattern corresponds to the random value
        foreach (var pattern in patterns)
        {
            cumulativeWeight += pattern.Value;
            if (randomValue < cumulativeWeight)
            {
                return pattern.Key; // Return the selected pattern
            }
        }

        return patterns.Keys.First(); // Fallback in case something goes wrong
    }


    private BuffSelectionUI buffSelectionUI; // Reference to the buff selection UI
    private CurrencyManager currencyManager;

    void Reward()
    {
        if (buffSelectionUI != null)
        {
            buffSelectionUI.ShowBuffChoices();
        }
        if (currencyManager != null)
        {
            currencyManager.AddCurrency(30);
        }
    }

    void FireLeft(int count)
    {
        List<GameObject> selectedShooters = GetRandomShooters(leftShooters, count);
        foreach (GameObject shooter in selectedShooters)
        {
            EnemyProjectileLauncher launcher = shooter.GetComponent<EnemyProjectileLauncher>();
            if (launcher != null)
            {
                launcher.isVerticalShooter = false; // Ensure it's set to horizontal
                launcher.minSpeed += projectileSpeedIncrement; // Apply the speed increment
                launcher.maxSpeed += projectileSpeedIncrement;
                launcher.FireProjectile();
            }
        }
    }

    void FireRight(int count)
    {
        List<GameObject> selectedShooters = GetRandomShooters(rightShooters, count);
        foreach (GameObject shooter in selectedShooters)
        {
            EnemyProjectileLauncher launcher = shooter.GetComponent<EnemyProjectileLauncher>();
            if (launcher != null)
            {
                launcher.isVerticalShooter = false; // Ensure it's set to horizontal
                launcher.minSpeed += projectileSpeedIncrement; // Apply the speed increment
                launcher.maxSpeed += projectileSpeedIncrement;
                launcher.FireProjectile();
            }
        }
    }

    void FireVertical(int count)
    {
        List<GameObject> selectedShooters = GetRandomShooters(verticalShooters, count);
        foreach (GameObject shooter in selectedShooters)
        {
            EnemyProjectileLauncher launcher = shooter.GetComponent<EnemyProjectileLauncher>();
            if (launcher != null)
            {
                launcher.isVerticalShooter = true; // Ensure it's set to vertical
                launcher.minSpeed += projectileSpeedIncrement; // Apply the speed increment
                launcher.maxSpeed += projectileSpeedIncrement;
                launcher.FireProjectile();
            }
        }
    }

    List<GameObject> GetRandomShooters(List<GameObject> shooters, int count)
    {
        List<GameObject> selected = new List<GameObject>();

        if (shooters.Count <= count)
        {
            return new List<GameObject>(shooters); // Return all if count is too high
        }

        while (selected.Count < count)
        {
            int index = Random.Range(0, shooters.Count);
            GameObject shooter = shooters[index];
            if (!selected.Contains(shooter))
            {
                selected.Add(shooter);
            }
        }
        return selected;
    }

    // Coroutine to activate the portal after a delay
    IEnumerator ActivatePortalWithDelay()
    {
        yield return new WaitForSeconds(portalDelay); // Wait for the specified delay
        ActivatePortal();
    }

    // Method to activate the portal when all waves are completed
    void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);  // Activate the portal
            Reward();
            Debug.Log("Portal has been activated!");
        }
    }

    // Method to heal the Damageable GameObject
    void HealDamageable()
    {
        if (damageableTarget != null)
        {
            Damageable damageable = damageableTarget.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Heal(5);
            }
            else
            {
                Debug.LogWarning("Damageable component not found on the target.");
            }
        }
        else
        {
            Debug.LogWarning("No Damageable target assigned.");
        }
    }
}
