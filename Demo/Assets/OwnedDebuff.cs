using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedDebuff : MonoBehaviour
{
    public List<BadEffect> activeDebuff = new List<BadEffect>();
    private Knight knight;
    void Awake()
    {
        knight = GetComponent<Knight>();

        if (knight == null)
        {
            Debug.LogError("Knight component not found!");
        }
    }

    void Start()
    {
        if (knight == null)
        {
            Debug.LogError("PlayerController not found!");
        }
        //ActivateDebuff();
    }

    //private void Update()
    //{
    //    foreach (BadEffect debuff in activeDebuff)
    //    {
    //        debuff.currentCooldown -= Time.deltaTime;
    //        if (debuff.currentCooldown <= 0)
    //        {
    //            debuff.currentCooldown = 0;
    //        }
    //    }
    //}   

    public Boolean AddDebuff(BadEffect debuff)
    {
        // Check if this debuff is already active
        BadEffect existingDebuff = activeDebuff.Find(d => d.id == debuff.id);

        if (existingDebuff == null)
        {
            //if (debuff.currentCooldown > 0)
            //{
            //    return false;
            //}
            // Add the new debuff
            activeDebuff.Add(debuff);
            //debuff.currentCooldown = debuff.cooldown;
            return true;
        }
        else return false;
    }

    public void RemoveDebuff(int debuffId)
    {
        BadEffect debuffToRemove = activeDebuff.Find(d => d.id == debuffId);
        if (debuffToRemove != null)
        {
            activeDebuff.Remove(debuffToRemove);
        }
    }
    //public void ActivateDebuff()
    //{
    //    foreach (BadEffect p in activeDebuff)
    //    {
    //        if (p is Metal_2_DB metalDebuff)
    //        {
    //            metalDebuff.ApplyDefenseReduction(knight);
    //        }
    //    }
    //}
}
