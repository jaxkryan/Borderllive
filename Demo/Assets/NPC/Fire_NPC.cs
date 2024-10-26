using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Fire_NPC", menuName = "NPC/Fire_NPC")]

public class Fire_NPC : Elemental_NPC
{
    private void OnEnable()
    {
        this.id = 1;
        InitializeLocalization("Fire_NPC_Name", "Fire_NPC_Description");
    }
}
