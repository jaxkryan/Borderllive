using UnityEngine;
using UnityEngine.UI; // For working with UI elements
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ShootingEvent : MonoBehaviour
{
    public List<GameObject> horizontalShooters; // List of horizontal shooters
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

                timer = 3f;               // Set timer for next wave to normal interval
                if (messageText != null)
                {
                    messageText.gameObject.SetActive(false); // Deactivate the messageText after the first wave starts
                }
            }

            // Handle subsequent waves normally after the first wave
            if (firstWaveStarted && timer <= 0f)
            {
                FireRandomPattern();
                timer = Random.Range(2, 5);
                currentWave++;

                // Check if 5 rounds have been completed, then heal
                if (currentWave % 5 == 0)
                {
                    HealDamageable(); // Call healing after every 5 rounds
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
        int pattern = Random.Range(0, 5); // There are 4 possible patterns

        switch (pattern)
        {
            case 0: // 1 horizontal
                FireHorizontal(1);
                break;
            case 1: // 2 vertical
                FireVertical(2);
                break;
            case 2: // 1 horizontal and 1 vertical
                FireHorizontal(1);
                FireVertical(1);
                break;
            case 3: // 1 horizontal and 2 vertical
                FireHorizontal(1);
                FireVertical(2);
                break;
            case 4: // 3 vertical
                FireVertical(3);
                break;
        }
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

    void FireHorizontal(int count)
    {
        List<GameObject> selectedShooters = GetRandomShooters(horizontalShooters, count);
        foreach (GameObject shooter in selectedShooters)
        {
            EnemyProjectileLauncher launcher = shooter.GetComponent<EnemyProjectileLauncher>();
            if (launcher != null)
            {
                launcher.isVerticalShooter = false; // Ensure it's set to horizontal
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

    // Method to heal the Damageable GameObject after every 5 rounds
    void HealDamageable()
    {
        if (damageableTarget != null)
        {
            Damageable damageable = damageableTarget.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Heal(20); // Heal the Damageable by 10
                Debug.Log("Damageable target healed by 10 after 5 rounds.");
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
