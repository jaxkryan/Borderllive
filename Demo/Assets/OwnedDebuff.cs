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
