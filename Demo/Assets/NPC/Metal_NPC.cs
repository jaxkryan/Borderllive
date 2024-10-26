using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Metal_NPC", menuName = "NPC/Metal_NPC")]

public class Metal_NPC : Elemental_NPC
{
    private void OnEnable()
    {
        this.id = 3;
        InitializeLocalization("Metal_NPC_Name", "Metal_NPC_Description");
    }
}
