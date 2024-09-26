using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int baseDamage = 10; // Base damage of the projectile
    public int additionalDamage = 0; // Additional damage that can be modified externally
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the velocity of the projectile based on its direction
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    // Trigger when the projectile collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // Calculate total damage (baseDamage + additionalDamage)
            int totalDamage = baseDamage + additionalDamage;

            // Determine the direction of knockback
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Apply the damage and knockback to the Damageable object
            bool gotHit = damageable.Hit(totalDamage, deliveredKnockback);

            // If the object got hit, log the damage dealt
            if (gotHit)
                Debug.Log(collision.name + " hit for " + totalDamage + " damage");

            // Destroy the projectile after hitting
            Destroy(gameObject);
        }
    }

    // Method to set additional damage from another script
    public void SetAdditionalDamage(int extraDamage)
    {
        additionalDamage = extraDamage;
    }
}
