using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedDebuff : MonoBehaviour
{
    public List<BadEffect> activeDebuff = new List<BadEffect>();
    private Knight knight;
    void Start()
    {
        knight = GetComponent<Knight>();

        if (knight == null)
        {
            Debug.LogError("PlayerController not found!");
        }
        ActivateDebuff();
    }

    public void AddDebuff(BadEffect debuff)
    {
        // Check if this debuff is already active
        BadEffect existingDebuff = activeDebuff.Find(d => d.GetType() == debuff.GetType());

        if (existingDebuff != null)
        {
            // Add the new debuff
            activeDebuff.Add(debuff);
            ActivateDebuff();
        }
    }


    public void ActivateDebuff()
    {
        foreach (BadEffect p in activeDebuff)
        {
            if (p is Metal_2_DB metalDebuff)
            {
                metalDebuff.ApplyDefenseReduction(knight);
            }
        }
    }
}
