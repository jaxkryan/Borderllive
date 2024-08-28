using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{
    public GameObject firePrefab;
    public Transform launchPoint;
    public string skillTag = "";
    public SpellCooldown spellCooldown;

    void Start()
    {
        GameObject cooldownObject = GameObject.FindWithTag(skillTag);
        if (cooldownObject != null)
        {
            spellCooldown = cooldownObject.GetComponent<SpellCooldown>();
        }
    }

    public void FireCast()
    {
        if (spellCooldown != null && spellCooldown.UseSpell())
        {
            GameObject fireInstance = Instantiate(firePrefab, launchPoint.position, firePrefab.transform.rotation);
            Destroy(fireInstance, 0.5f);
        }
    }
}
