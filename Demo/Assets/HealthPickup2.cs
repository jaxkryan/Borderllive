using UnityEngine;

public class HealthPickup2 : MonoBehaviour
{
    // public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable && damageable.Health < damageable.MaxHealth)
        {
            // Calculate the heal amount based on the player's current health
            int healAmount = CalculateHealAmount(damageable);

            bool wasHealed = damageable.Heal(healAmount);

            if (wasHealed)
                if (pickupSource)
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
            Destroy(gameObject);
        }
    }

    // New method to calculate the heal amount
    private int CalculateHealAmount(Damageable damageable)
    {
        // Define a minimum and maximum heal amount
        int minHeal = (int)(0.1 * damageable.MaxHealth);
        int maxHeal = (int)(0.5 * damageable.MaxHealth);

        // Calculate the heal amount based on the player's current health
        float healthRatio = (float)damageable.Health / damageable.MaxHealth;
        int healAmount = (int)Mathf.Lerp(maxHeal, minHeal, healthRatio);

        return healAmount;
    }
    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
