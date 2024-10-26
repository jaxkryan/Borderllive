using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Water_NPC", menuName = "NPC/Water_NPC")]

public class Water_NPC : Elemental_NPC
{
    private void OnEnable()
    {
        this.id = 4;
        InitializeLocalization("Water_NPC_Name", "Water_NPC_Description");
    }
}
