using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public int baseDamage = 10; // Base damage of the projectile
    public int additionalDamage = 0; // Additional damage that can be modified externally
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    public Vector2 endPoint; // End point for the projectile
    private bool hasExploded = false; // To track if the projectile has already exploded

    private Rigidbody2D rb;

    public GameObject explosionEffectPrefab; // Assign the explosion effect prefab in the Inspector

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        // Set the initial velocity towards the endpoint
        Vector2 direction = (endPoint - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed.x;
    }


    private void Update()
    {
        // Check if the projectile has reached the endpoint
        float distance = Vector2.Distance(transform.position, endPoint);

        // Adjust this value if needed
        float explosionDistanceThreshold = 0.1f;

        if (!hasExploded && distance <= explosionDistanceThreshold)
        {
            Explode();
        }
    }
    void FixedUpdate()
    {
        // Move towards the endpoint
        if (!hasExploded)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint, moveSpeed.x * Time.fixedDeltaTime);

            // Check if the projectile has reached the endpoint
            float distance = Vector2.Distance(transform.position, endPoint);
            if (distance <= 0.1f)
            {
                Explode();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision and apply damage
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // If it hits a Damageable object, explode immediately
            Explode(); // Call the explode method
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Prevent double explosion
        hasExploded = true;

        // Visualize explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Offset the explosion position slightly to avoid affecting unwanted colliders below
        Vector2 explosionPosition = transform.position + Vector3.up * 1f; // Adjust this offset as needed

        // Find all colliders within a certain radius (you can adjust the radius)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, 3f); // Adjust radius as needed

        foreach (Collider2D collider in colliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null && damageable.GetComponent<PlayerController>() == null)
            {
                // Calculate total damage (baseDamage + additionalDamage)
                int totalDamage = baseDamage + additionalDamage;

                // Apply the damage to the Damageable object
                damageable.Hit(totalDamage, knockback);
                Debug.Log(collider.name + " hit for " + totalDamage + " damage");
            }
        }

        // Destroy the projectile after exploding
        Destroy(gameObject);
    }


    public void SetAdditionalDamage(int extraDamage)
    {
        additionalDamage = extraDamage;
    }
}
