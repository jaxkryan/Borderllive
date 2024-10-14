using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleManager : MonoBehaviour
{
    // Reference to the boss GameObject (you can assign this in the Inspector)
    public GameObject boss;

    // Reference to the exit GameObject (to be enabled when the boss dies)
    public GameObject exit;

    // Reference to the slider (health bar) to represent the boss's health
    public Slider bossHealthSlider;

    // Reference to the Damageable component on the boss
    public Damageable bossDamageable;

    // Reference to the EnemyXpTracker component on the boss
    public EnemyXPTracker bossXpTracker;

    void Awake()
    {
        // Get the EnemyXpTracker component from the boss and immediately add XP
        if (boss != null)
        {
            if (bossXpTracker != null)
            {
                // Add XP with amount = 0 when the script wakes up
                bossXpTracker.AddXP(0);
                Debug.Log("Add " + bossXpTracker.XPTranslation.CurrentXP);
            }
        }
    }

    void Start()
    {
        // Ensure the exit is initially disabled
        if (exit != null)
        {
            exit.SetActive(false);
        }

        // Get the Damageable component from the boss
        if (boss != null)
        {

            // Initialize the health slider's max value to the boss's MaxHealth
            if (bossDamageable != null && bossHealthSlider != null)
            {
                bossHealthSlider.maxValue = bossDamageable.MaxHealth;
                bossHealthSlider.value = bossDamageable.Health;
            }
        }
    }

    void Update()
    {
        if (boss != null && bossDamageable != null)
        {
            // Update the slider value to match the boss's current health
            bossHealthSlider.value = bossDamageable.Health;
            bossHealthSlider.maxValue = bossDamageable.MaxHealth;
            // Check if the boss is dead (when health reaches 0 or below)
            if (bossDamageable.Health <= 0)
            {
                // Enable the exit
                exit.SetActive(true);

                // Call AddXP with amount = 0 when the boss dies
                if (bossXpTracker != null)
                {
                    bossXpTracker.AddXP(0);
                }

                // Optionally, disable this script since it's no longer needed
                this.enabled = false;
            }
        }
    }
}