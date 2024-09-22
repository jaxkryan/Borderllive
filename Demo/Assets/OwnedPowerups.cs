using System.Collections.Generic;
using UnityEngine;

public class OwnedPowerups : MonoBehaviour
{
    public List<Powerups> activePowerups = new List<Powerups>();
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found!");
        }
        ActivatePowerup();
    }

    //public void AddPowerup(Powerups powerup)
    //{
    //    if (!activePowerups.Contains(powerup))
    //    {
    //        activePowerups.Add(powerup);
    //        ActivatePowerup(powerup); // Automatically activate when added
    //    }
    //}

    public void ActivatePowerup()
    {
        Debug.Log("so pu: " + activePowerups.Count);
        foreach (Powerups p in activePowerups) {
            // Apply Metal_1 effect to the player
            if (p is Metal_1 metalPowerup)
            {
                metalPowerup.ApplyEffect(playerController);
            }
        }
    }
}