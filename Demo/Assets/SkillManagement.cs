using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>();

    // Currently selected skills (max 3)
    public List<Skill> selectedSkills = new List<Skill>();

    // Reference to the player controller
    public PlayerController playerController;

    // Reference to the SpellCooldown script
    // public SpellCooldown spellCooldown;

    void Start()
    {
        // Initialize the selected skills list
        for (int i = 0; i < 3; i++)
        {
            selectedSkills.Add(null);
        }
    }

    // Called when the player wants to select a new skill
    public void SelectSkill(Skill skill)
    {
        // Check if the player already has 3 skills
        if (selectedSkills.Count >= 3)
        {
            // Prompt the player to choose which skill to replace
            ShowReplaceSkillMenu(skill);
        }
        else
        {
            // Add the new skill to the selected skills list
            selectedSkills.Add(skill);
            UpdatePlayerController();
        }
    }

    // Called when the player wants to replace a skill
    public void ReplaceSkill(Skill oldSkill, Skill newSkill)
    {
        // Remove the old skill from the selected skills list
        selectedSkills.Remove(oldSkill);
        // Add the new skill to the selected skills list
        selectedSkills.Add(newSkill);
        UpdatePlayerController();
    }

    // Update the player controller with the new selected skills
    void UpdatePlayerController()
    {
        // Update the player controller's skills
        playerController.skill1Trigger = GetSkillTrigger("OnSkill1");
        playerController.skill2Trigger = GetSkillTrigger("OnSkill2");
        playerController.skill3Trigger = GetSkillTrigger("OnSkill3");
    }

    // Get a skill by name
    string GetSkillTrigger(string skillName)
    {
        foreach (Skill skill in selectedSkills)
        {
            if (skill.name == skillName)
            {
                return skill.triggerName;
            }
        }
        return null;
    }

    // Show a menu to choose which skill to replace
    void ShowReplaceSkillMenu(Skill newSkill)
    {
        // Create a menu with the currently selected skills
        // ...
        // When the player chooses a skill to replace, call ReplaceSkill(oldSkill, newSkill)
    }
}
