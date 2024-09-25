using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedPowerups : MonoBehaviour
{
    public List<Powerups> activePowerups = new List<Powerups>();
    private PlayerController playerController;
    public bool isHitEnemy = false; // Biến cờ để kiểm tra khi tấn công trúng kẻ địch
    public int hitCount;
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

    public string SerializeActivePowerups()
    {
        return JsonUtility.ToJson(this);
    }

    public void DeserializeActivePowerups(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }

    //check if the powerup in activePowerups or not
    public bool IsPowerupActive<T>() where T : Powerups
    {
        foreach (Powerups p in activePowerups)
        {
            if (p is T)
            {
                return true;
            }
        }
        return false;
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
            if (p is Wood_3 wood3Powerup)
            {
                wood3Powerup.ApplyEffect(playerController);
            }
            if (p is Water_2 water2Powerup)
            {
                water2Powerup.ApplyEffect(playerController);
            }
        }
    }
    //activate a permanent buff
    public void ActivateAPowerup(Powerups p)
    {

        if (p is Metal_1 metalPowerup)
        {
            metalPowerup.ApplyEffect(playerController);
        }
        if (p is Wood_3 wood3Powerup)
        {
            wood3Powerup.ApplyEffect(playerController);
        }
        if (p is Water_2 water2Powerup)
        {
            water2Powerup.ApplyEffect(playerController);
        }
    }

    //condition debuff go here
    public void CheckPowerupEffects(Knight enemyKnight)
    {
        GameObject enemyKnightGameObject = enemyKnight.gameObject;
        OwnedDebuff ownedDebuff = enemyKnightGameObject.GetComponent<OwnedDebuff>();
        //Debug.Log("number of pu : " + activePowerups.Count);
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
                    //Debug.Log("hit count: " + hitCount);
                    if (hitCount == 8)
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
                
                if (p is Water_1 water)
                {
                    {
                        if (water.ApplyEffect(enemyKnight))
                        {
                            Water_1_DB newDebuff = new Water_1_DB();
                            //Debug.Log("duration of debuff: " + newDebuff.duration);
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
        hitCount++;
    }

    public void ResetHit()
    {
        isHitEnemy = false;
    }

    //buff with condition - metal 3
    internal void TriggerMetal3Buff()
    {
        Metal_3 metal_3 = new Metal_3();
        metal_3.ApplyEffect(playerController);
        //Debug.Log("trigger metal 3 buff is ok");
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

    //buff w/ condition - wood 3
    internal void TriggerWood3Buff()
    {
       Wood_3 wood_3 = new Wood_3();
       wood_3.ApplyEffect(playerController);
    }
}
