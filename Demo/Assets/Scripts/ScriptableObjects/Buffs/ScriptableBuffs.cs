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

    /**
     * Duration is increased each time the buff is applied.
     */
    public bool IsDurationStacked;

    /**
     * Effect value is increased each time the buff is applied.
     */
    public bool IsEffectStacked;

}
