using UnityEngine;
using UnityEngine.Localization;


public class Elemental_NPC : ScriptableObject
{
    public int id;
    public LocalizedString nameLocalization;
    public LocalizedString descriptionLocalization;
    public Sprite npcSprite;  // New Sprite field for the NPC's image

    public void InitializeLocalization(string nameKey, string descriptionKey)
    {
        nameLocalization = new LocalizedString { TableReference = "Elemental_NPC", TableEntryReference = nameKey };
        descriptionLocalization = new LocalizedString { TableReference = "Elemental_NPC", TableEntryReference = descriptionKey };
    }

    public void UpdateNameLocalization(string nameKey)
    {
        nameLocalization.TableEntryReference = nameKey;
    }

    public void UpdateDescriptionLocalization(string descriptionKey)
    {
        descriptionLocalization.TableEntryReference = descriptionKey;
    }
}
