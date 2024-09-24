using System.Collections.Generic;
using UnityEngine;

public class BuffPool : MonoBehaviour
{
    // Static list to keep track of available power-ups globally
    public static List<Powerups> availablePowerups = new List<Powerups>();

    // List of predefined power-ups (reference these in the Inspector if needed)
    [SerializeField] private List<Powerups> initialPowerups = new List<Powerups>();

    private void Awake()
    {
        // Initialize availablePowerups with initial power-ups on game start
        ResetBuffPool();
    }

    // Method to reset the buff pool with the initial set of power-ups
    public void ResetBuffPool()
    {
        availablePowerups.Clear(); // Clear any previous data
        availablePowerups.AddRange(initialPowerups); // Refill the pool with the initial buffs
    }

    // Method to remove a selected buff from the list
    public void RemoveBuff(Powerups selectedBuff)
    {
        availablePowerups.Remove(selectedBuff);
    }

    // Method to get random buffs without repetition
    public Powerups[] GetRandomBuffs(int count)
    {
        if (availablePowerups.Count < count)
            return availablePowerups.ToArray();

        List<Powerups> randomBuffs = new List<Powerups>();
        while (randomBuffs.Count < count)
        {
            Powerups randomBuff = availablePowerups[Random.Range(0, availablePowerups.Count)];
            if (!randomBuffs.Contains(randomBuff))
                randomBuffs.Add(randomBuff);
        }
        return randomBuffs.ToArray();
    }
}
