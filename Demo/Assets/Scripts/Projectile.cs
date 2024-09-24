using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10; // Initial base damage of the projectile
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    private Rigidbody2D rb;
    private CharacterStat characterStat; // Reference to get Damage stat

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Set CharacterStat when the projectile is instantiated
    public void SetCharacterStat(CharacterStat shooterStat)
    {
        characterStat = shooterStat;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // Get additional damage from the CharacterStat, if available
            float additionalDamage = characterStat != null ? characterStat.Damage : 0; // Adjust based on actual Damage field name
            int totalDamage = damage + (int) additionalDamage; // Combine base damage with additional damage from stats

            // Determine the direction of knockback
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Apply the combined damage to the Damageable component
            bool gotHit = damageable.Hit(totalDamage, deliveredKnockback);

            if (gotHit)
                Debug.Log(collision.name + " hit for " + totalDamage + " damage");

            Destroy(gameObject);
        }
    }
}
