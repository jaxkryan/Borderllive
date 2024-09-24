public interface IDebuffable
{
    public void ApplyDebuff(ScriptableDebuff debuff);
    public void RemoveDebuff();
    public void HandleDebuff();
}
