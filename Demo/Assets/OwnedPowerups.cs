﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpData
{
    public int id;
    public int elementId;
    public Powerups.BuffType buffType;
    public Powerups.TriggerCondition triggerCondition;
    public Powerups.Effect effect;
    public int weight;
    public int berserkRateIncrease;
    public int cooldown;
    public int duration;
    public bool isActive;
    public float currentCooldown;
    public string description;

    // Add a field for the specific type
    public string typeName; // This will store the name of the type, e.g., "Metal_1"
}

[System.Serializable]
public class SerializationHelper<T>
{
    public List<T> list;

    public SerializationHelper(List<T> list)
    {
        this.list = list;
    }
}

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

    public void SavePowerups()
    {
        List<PowerUpData> powerUpDataList = new List<PowerUpData>();

        foreach (Powerups powerup in activePowerups)
        {
            PowerUpData data = new PowerUpData()
            {
                id = powerup.id,
                elementId = powerup.ElementId,
                buffType = powerup.type,
                triggerCondition = powerup.triggerCondition,
                effect = powerup.effect,
                weight = powerup.Weight,
                berserkRateIncrease = powerup.BerserkRateIncrease,
                cooldown = powerup.cooldown,
                duration = powerup.duration,
                isActive = powerup.isActive,
                currentCooldown = powerup.currentCooldown,
                description = powerup.description,
                typeName = powerup.GetType().Name // Store the type name
            };
            powerUpDataList.Add(data);
        }

        string json = JsonUtility.ToJson(new SerializationHelper<PowerUpData>(powerUpDataList));
        PlayerPrefs.SetString("ActivePowerups", json);
        PlayerPrefs.Save();
    }

    public void LoadPowerups()
    {
        if (PlayerPrefs.HasKey("ActivePowerups"))
        {
            string json = PlayerPrefs.GetString("ActivePowerups");
            List<PowerUpData> powerUpDataList = JsonUtility.FromJson<SerializationHelper<PowerUpData>>(json).list;

            activePowerups.Clear(); // Clear current power-ups before loading new ones
            foreach (PowerUpData data in powerUpDataList)
            {
                // Use reflection to create an instance of the correct type
                Type powerupType = Type.GetType(data.typeName);
                if (powerupType != null)
                {
                    Powerups powerup = (Powerups)Activator.CreateInstance(powerupType);
                    powerup.id = data.id;
                    powerup.ElementId = data.elementId;
                    powerup.type = data.buffType;
                    powerup.triggerCondition = data.triggerCondition;
                    powerup.effect = data.effect;
                    powerup.Weight = data.weight;
                    powerup.BerserkRateIncrease = data.berserkRateIncrease;
                    powerup.cooldown = data.cooldown;
                    powerup.duration = data.duration;
                    powerup.isActive = data.isActive;
                    powerup.currentCooldown = data.currentCooldown;
                    powerup.description = data.description;

                    activePowerups.Add(powerup);
                }
                else
                {
                    Debug.LogWarning($"Powerup type {data.typeName} not found!");
                }
            }
        }
    }



    // Clear powerups when the player dies or quits
    public void ClearPowerups()
    {
        activePowerups.Clear();
        PlayerPrefs.DeleteKey("ActivePowerups"); // Remove saved powerups from PlayerPrefs
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
    public bool IsBuffActive(Powerups powerups)
    {
        return activePowerups.Contains(powerups);
    }

    //wtf code cho nay xau vcl
    //direct buff without condition go here 
    public void ActivatePowerup()
    {
        var uniquePowerupIds = new HashSet<int>();
        var uniquePowerups = new List<Powerups>();

        if (activePowerups.Count > 0)
        {
            foreach (Powerups p in activePowerups)
            {
                if (uniquePowerupIds.Add(p.id))
                {
                    uniquePowerups.Add(p);
                }
            }
        }
        activePowerups = uniquePowerups;
        foreach (Powerups p in activePowerups)
        {
            ActivateAPowerup(p);
        }
    }
    //activate a permanent buff after choosing
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
        if (p is Water_3 water3Powerup)
        {
            water3Powerup.ApplyEffect(playerController);
        }
        if (p is Fire_3 fire3Powerup)
        {
            fire3Powerup.ApplyEffect(playerController);
        }
        if (p is Earth_1 earth1Powerup)
        {
            earth1Powerup.ApplyEffect(playerController);
        }
        if (p is Earth_2 earth2Powerup)
        {
            earth2Powerup.ApplyEffect(playerController);
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

    internal void TriggerEarth2Buff()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        Earth_2 e2 = new Earth_2();
        e2.ApplyEffect(playerController);
    }

    //buff w/ condition - wood 3
    internal void TriggerWood3Buff()
    {
        Wood_3 wood_3 = new Wood_3();
        wood_3.ApplyEffect(playerController);
    }

    internal void RemoveEarth2Buff()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.DecreaseDef(0.2f);
            Debug.Log("Earth_2 buff removed as health is under 50%!");
        }
    }

    private bool isEarth3BuffTriggered = false;

    internal void TriggerEarth3Buff()
    {
        if (!isEarth3BuffTriggered)
        {
            Earth_3 e3 = new Earth_3();
            e3.ApplyEffect(playerController);
            isEarth3BuffTriggered = true;
        }
    }

    // Call this method when the player enters a new room
    public void ResetEarth3Buff()
    {
        isEarth3BuffTriggered = false;
    }
}
