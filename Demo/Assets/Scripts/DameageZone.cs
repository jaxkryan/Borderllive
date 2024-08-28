using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 20;
    public bool sendPlayerToOriginalLocation = true;
    public Vector2 playerOriginalPosition;
    public float knockbackForce = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply knockback force
                Vector2 knockback = knockbackDirection * knockbackForce;

                // Use Hit method with damage amount and calculated knockback
                damageable.Hit(damageAmount, knockback);

                if (sendPlayerToOriginalLocation)
                {
                    other.transform.position = playerOriginalPosition; // Send player to original position
                }
            }
        }
        else {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null && sendPlayerToOriginalLocation)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply knockback force
                Vector2 knockback = knockbackDirection * knockbackForce;

                // Use Hit method with damage amount and calculated knockback
                damageable.Hit(1000, knockback);

               
            }
        }
    }
}
