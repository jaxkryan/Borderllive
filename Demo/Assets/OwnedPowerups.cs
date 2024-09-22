using System.Collections.Generic;
using UnityEngine;

public class OwnedPowerups : MonoBehaviour
{
    public List<Powerups> activePowerups = new List<Powerups>();
    private PlayerController playerController;
    private Knight knight;
    private bool isHitEnemy = false; // Biến cờ để kiểm tra khi tấn công trúng kẻ địch

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        knight = GetComponent<Knight>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found!");
        }
        ActivatePowerup();
    }

    public void ActivatePowerup()
    {
        foreach (Powerups p in activePowerups)
        {
            if (p is Metal_1 metalPowerup)
            {
                metalPowerup.ApplyEffect(playerController);
            }
        }
    }

    void Update()
    {
        CheckPowerupEffects(knight);
        ResetHit();
    }

    public void CheckPowerupEffects(Knight enemyKnight)
    {
        foreach (Powerups p in activePowerups)
        {
            if (p is Metal_2 metalPowerup2)
            {
                if (isHitEnemy)
                {
                    metalPowerup2.ApplyEffect(enemyKnight);
                }
            }
        }
    }

    public void EnemyHit()
    {
        isHitEnemy = true;
    }

    public void ResetHit()
    {
        isHitEnemy = false;
    }
}
