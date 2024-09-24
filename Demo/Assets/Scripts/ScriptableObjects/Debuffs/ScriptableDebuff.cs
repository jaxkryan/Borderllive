using UnityEngine;

[CreateAssetMenu(menuName = "Debuff")]
public class ScriptableDebuff : ScriptableObject
{
    public string Name;
    public int Value;
    public float TickSpeed;
    /**
     * Time duration of the Debuff in seconds.
     */
    public float Duration;

    /**
     * Duration is increased each time the Debuff is applied.
     */
    public bool IsDurationStacked;

    /**
     * Effect value is increased each time the Debuff is applied.
     */
    public bool IsEffectStacked;

}
