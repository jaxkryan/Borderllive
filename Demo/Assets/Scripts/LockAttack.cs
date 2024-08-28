using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    public float stunDuration = 2f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spellCooldown != null && spellCooldown.UseSpell()) // Check if the spell can be used
        {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null)
            {
                Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

                if (gotHit)
                {
                    Debug.Log(collision.name + " stunning");
                    damageable.stunDuration = stunDuration;
                    damageable.IsStun = true;
                }
            }
        }
    }
}
