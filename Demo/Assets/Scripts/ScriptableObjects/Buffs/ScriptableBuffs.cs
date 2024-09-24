using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff")]
public class ScriptableBuff : ScriptableObject
{
    public string Name;
    public int Value;
    public float TickSpeed;

    /**
     * Time duration of the buff in seconds.
     */
    public float Duration;
}
