using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10;
    public bool sendPlayerToOriginalLocation = true;
    public Vector2 playerOriginalPosition;
    public float knockbackForce = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            CharacterStat stat = other.GetComponent<CharacterStat>();
            if (damageable != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply knockback force
                Vector2 knockback = knockbackDirection * knockbackForce;
                int totalDamage = damageAmount + (int) stat.DEF + (int)(damageable.MaxHealth * 0.2f);
                // Use Hit method with damage amount and calculated knockback
                damageable.Hit(totalDamage, knockback);

                if (sendPlayerToOriginalLocation)
                {
                    other.transform.position = playerOriginalPosition; // Send player to original position
                }
            }
        }
        else
        {
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
