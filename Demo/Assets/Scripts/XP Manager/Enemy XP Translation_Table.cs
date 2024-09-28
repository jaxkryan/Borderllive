using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyXPTranslationTableEntry
{
    public int Level;
    public int XPRequired;
}

[CreateAssetMenu(menuName = "RPG/Enemy XP Table", fileName = "EnemyXPTranslation_Table")]
public class EnemyXPTranslation_Table : BaseXPTranslation
{
    [SerializeField] List<EnemyXPTranslationTableEntry> Table;

    private void OnEnable()
    {
        // Check if the table is already populated
        if (Table == null || Table.Count == 0)
        {
            PopulateTable();
        }
    }

    private void PopulateTable()
    {
        Table = new List<EnemyXPTranslationTableEntry>();

        Table.Add(new EnemyXPTranslationTableEntry { Level = 1, XPRequired = 0 });
        // Level 1 to Level 5 with a fixed XP requirement of 200
        for (int level = 2; level <= 5; level++)
        {
            Table.Add(new EnemyXPTranslationTableEntry { Level = level, XPRequired = 200 * (level - 1) });
        }

        // Variables for the calculation for levels beyond 5
        int lastXPRequired = 800; // Start from 1000 for level 6
        int increaseAmount = 100;   // Initial increase amount

        for (int level = 6; level <= 50; level++)
        {
            // Calculate XP required for the current level
            lastXPRequired += increaseAmount;
            Table.Add(new EnemyXPTranslationTableEntry { Level = level, XPRequired = lastXPRequired });

            // Change the increase amount every 5 levels
            if (level % 5 == 0)
            {
                increaseAmount += 70; // Increase the increase amount
            }
        }
    }


    public override bool AddXP(int amount)
    {
        if (AtLevelCap)
            return false;

        CurrentXP += amount;
        for (int index = Table.Count - 1; index >= 0; index--)
        {
            var entry = Table[index];

            // found a matching entry
            if (CurrentXP >= entry.XPRequired)
            {
                // level changed?
                if (entry.Level != CurrentLevel)
                {
                    CurrentLevel = entry.Level;

                    AtLevelCap = Table[^1].Level == CurrentLevel;

                    return true;
                }
                break;
            }
        }

        return false;
    }

    public override void SetLevel(int level)
    {
        CurrentXP = 0;
        CurrentLevel = 1;
        AtLevelCap = false;

        foreach (var entry in Table)
        {
            if (entry.Level == level)
            {
                AddXP(entry.XPRequired);
                return;
            }
        }

        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {level}");
    }

    protected override int GetXPRequiredForNextLevel()
    {
        if (AtLevelCap)
            return int.MaxValue;

        for (int index = 0; index < Table.Count; ++index)
        {
            var entry = Table[index];

            if (entry.Level == CurrentLevel)
                return Table[index + 1].XPRequired - CurrentXP;
        }

        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {CurrentLevel}");
    }

    public override int GetNextLevelXPRequirement()
    {
        if (AtLevelCap)
            return int.MaxValue;

        for (int index = 0; index < Table.Count; ++index)
        {
            var entry = Table[index];

            if (entry.Level == CurrentLevel)
                return Table[index + 1].XPRequired;
        }

        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {CurrentLevel}");
    }
    public override int GetXPRequiredForCurrentLevel()
    {
        if (AtLevelCap)
            return int.MaxValue;
       
        for (int index = 0; index < Table.Count; ++index)
        {
            var entry = Table[index];
            if (entry.Level == CurrentLevel)
                return Table[index].XPRequired;
        }


        throw new System.ArgumentOutOfRangeException($"Could not find any entry for level {CurrentLevel}");
    }

   
}