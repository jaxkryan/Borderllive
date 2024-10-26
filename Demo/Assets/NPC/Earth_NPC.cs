using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Earth_NPC", menuName = "NPC/Earth_NPC")]

public class Earth_NPC : Elemental_NPC
{
    private void OnEnable()
    {
        this.id = 2;
        InitializeLocalization("Earth_NPC_Name", "Earth_NPC_Description");
    }
}
