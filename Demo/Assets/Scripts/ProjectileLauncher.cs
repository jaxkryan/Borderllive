using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public string skillTag; // Tag for this skill

    public SpellCooldown spellCooldown; // Reference to the SpellCooldown script

    void Start()
    {
        GameObject cooldownObject = GameObject.FindWithTag(skillTag);
        if (cooldownObject != null)
        {
            spellCooldown = cooldownObject.GetComponent<SpellCooldown>();
        }
    }

    public void FireProjectile()
    {
        if (spellCooldown != null && spellCooldown.UseSpell()) // Check if the spell can be used
        {
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectile.transform.localScale;

            projectile.transform.localScale = new Vector3(
                origScale.x * (transform.localScale.x > 0 ? 1f : -1f),
                origScale.y,
                origScale.z
            );
        }
    }
}
