using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffPool : MonoBehaviour
{
    // Static list to keep track of available power-ups globally
    public static List<Powerups> availablePowerups = new List<Powerups>();

    // List of predefined power-ups (reference these in the Inspector if needed)
    [SerializeField] private List<Powerups> initialPowerups = new List<Powerups>();
    private OwnedPowerups ownedPowerups;
    private void Awake()
    {
        // Initialize availablePowerups with initial power-ups on game start
        ResetBuffPool();
        ownedPowerups = FindObjectOfType<OwnedPowerups>();

    }

    // Method to reset the buff pool with the initial set of power-ups
    public void ResetBuffPool()
    {
        availablePowerups.Clear(); // Clear any previous data
        availablePowerups.AddRange(initialPowerups); // Refill the pool with the initial buffs
        // foreach (Powerups powerups in availablePowerups)
        // {
        //     powerups.InitializeLocalization(powerups.name, powerups.description);
        // }
    }

    // Method to remove a selected buff from the list
    public void RemoveBuff(Powerups selectedBuff)
    {
        availablePowerups.Remove(selectedBuff);
    }

    // Method to get random buffs without repetition
    public Powerups[] GetRandomBuffs(int count)
    {
        List<Powerups> temp = new List<Powerups>();
        // Only add non-active powerups to the temporary list
        foreach (var powerup in availablePowerups)
        {
            if (!ownedPowerups.IsBuffActive(powerup))
            {
                temp.Add(powerup);
            }
        }

        List<Powerups> randomBuffs = new List<Powerups>();
        while (randomBuffs.Count < count && temp.Count > 0)
        {
            Powerups randomBuff = temp[Random.Range(0, temp.Count)];
            randomBuffs.Add(randomBuff);
            temp.Remove(randomBuff); // Ensure no duplicates
        }

        return randomBuffs.ToArray();
    }

}
