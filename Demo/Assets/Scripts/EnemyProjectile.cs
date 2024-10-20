using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int baseDamage = 30; // Base damage of the projectile
    public int additionalDamage = 0; // Additional damage that can be modified externally
    public bool maxHealthDamage = true; // Determines if the damage is based on max health

    public Vector2 direction = Vector2.right; // Default direction is to the right
    public float speed = 8f; // Speed of the projectile
    public Vector2 knockback = new Vector2(0, 0); // Knockback applied to hit objects

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the velocity of the projectile based on its direction
        rb.velocity = direction.normalized * speed;
    }

    // Trigger when the projectile collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            CharacterStat stat = collision.GetComponent<CharacterStat>();
            if (stat != null)
            {
                // Calculate total damage based on whether maxHealthDamage is true or false
                int totalDamage = baseDamage + additionalDamage + (int)stat.DEF;

                // Apply max health scaling if maxHealthDamage is true
                if (maxHealthDamage)
                {
                    totalDamage += (int)(stat.MaxHealth * 0.2f);
                }

                // Apply knockback in the same direction as the projectile's direction
                Vector2 deliveredKnockback = direction.normalized * knockback.magnitude;

                // Apply the damage and knockback to the Damageable object
                bool gotHit = damageable.Hit(totalDamage, deliveredKnockback);

                // If the object got hit, log the damage dealt
                if (gotHit)
                    Debug.Log(collision.name + " hit for " + totalDamage + " damage");

                // Destroy the projectile after hitting
                Destroy(gameObject);
            }
        }
    }

    // Method to set additional damage from another script
    public void SetAdditionalDamage(int extraDamage)
    {
        additionalDamage = extraDamage;
    }

    // Method to set the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Method to set whether max health scaling is applied
    public void SetMaxHealthDamage(bool enableMaxHealthDamage)
    {
        maxHealthDamage = enableMaxHealthDamage;
    }
}
