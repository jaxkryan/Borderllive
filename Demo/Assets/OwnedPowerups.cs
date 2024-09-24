using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedPowerups : MonoBehaviour
{
    public List<Powerups> activePowerups = new List<Powerups>();
    private PlayerController playerController;
    private bool isHitEnemy = false; // Biến cờ để kiểm tra khi tấn công trúng kẻ địch
    private int hitCount = 0;
    //private void Awake()
    //{

    //}
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found!");
        }
        ActivatePowerup();
    }

    //direct buff without condition go here 
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
    //activate a buff
    public void ActivateAPowerup(Powerups p)
    {

        if (p is Metal_1 metalPowerup)
        {
            metalPowerup.ApplyEffect(playerController);
        }

    }

    //condition buff/debuff go here
    public void CheckPowerupEffects(Knight enemyKnight)
    {
        GameObject enemyKnightGameObject = enemyKnight.gameObject;
        OwnedDebuff ownedDebuff = enemyKnightGameObject.GetComponent<OwnedDebuff>();
        foreach (Powerups p in activePowerups)
        {
            if (isHitEnemy)
            {
                if (p is Metal_2 metalPowerup2)
                {

                    {
                        if (metalPowerup2.ApplyEffect(enemyKnight))
                        {
                            Metal_2_DB newDebuff = new Metal_2_DB();
                            Debug.Log("duration of debuff: " + newDebuff.duration);
                            if (ownedDebuff != null)
                            {
                                ownedDebuff.AddDebuff(newDebuff);
                                ResetHit();
                                StartCoroutine(RemoveDebuffAfterDuration(ownedDebuff, newDebuff.id, newDebuff.duration + newDebuff.cooldown));
                            }
                            else
                            {
                                Debug.LogError("OwnedDebuff component not found on enemyKnight!");
                            }
                        }
                    }
                }

                if (p is Wood_2 woodPowerup2)
                {
                    hitCount++;
                    if (hitCount == 10)
                    {
                        woodPowerup2.ApplyEffect(enemyKnight);
                        if (ownedDebuff != null)
                        {
                            Wood_2_DB newDebuff = new Wood_2_DB();
                            ownedDebuff.AddDebuff(newDebuff);
                            ResetHit();
                            StartCoroutine(RemoveDebuffAfterDuration(ownedDebuff, newDebuff.id, newDebuff.duration + newDebuff.cooldown));
                        }
                        hitCount = 0;
                    }
                }
            }
        }
    }
    private IEnumerator RemoveDebuffAfterDuration(OwnedDebuff ownedDebuff, int debuffId, float duration)
    {
        yield return new WaitForSeconds((float)(duration));
        ownedDebuff.RemoveDebuff(debuffId);
    }

    public void EnemyHit()
    {
        isHitEnemy = true;
    }

    public void ResetHit()
    {
        isHitEnemy = false;
    }

    internal void TriggerMetal3Buff()
    {
        Metal_3 metal_3 = new Metal_3();
        metal_3.ApplyEffect(playerController);
    }

    internal void RemoveMetal3Buff()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.DecreaseDef(0.1f);
            Debug.Log("Metal_3 buff removed as health is above 50%!");
        }
    }
}
