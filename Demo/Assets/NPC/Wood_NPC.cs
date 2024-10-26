using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wood_NPC", menuName = "NPC/Wood_NPC")]

public class Wood_NPC : Elemental_NPC
{
    private void OnEnable()
    {
        this.id = 5;
        InitializeLocalization("Wood_NPC_Name", "Wood_NPC_Description");
    }
}
