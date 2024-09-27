using TMPro;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    // Stamina variables
    [SerializeField] float BaseStamina_PerLevel = 5;
    [SerializeField] float BaseStamina_Offset = 10;
    [SerializeField] float StaminaToHealthConversion = 10;

    // Strength variables
    [SerializeField] float BaseStrength_PerLevel = 3;
    [SerializeField] float BaseStrength_Offset = 5;
    [SerializeField] float StrengthToDamageConversion = 2;

    // Endurance variables
    [SerializeField] float BaseEndurance_PerLevel = 2;  // Endurance increases per level
    [SerializeField] float BaseEndurance_Offset = 2;    // Fixed offset added to Endurance
    [SerializeField] float EnduranceToDefConversion = 0.5f; // Conversion rate from Endurance to DEF

    // Speed variable
    [SerializeField] float BaseSpeed = 5; // Fixed value for Speed

    // UI elements
    //[SerializeField] TextMeshProUGUI StaminaText;
    [SerializeField] TextMeshProUGUI DamageText;
    //[SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI DefText;
    //[SerializeField] TextMeshProUGUI SpeedText; // UI for Speed (if needed)

    // Reference to the Damageable component
    private Damageable damageable;

   [SerializeField]
    private float shield;

    public float Shield
    {
        get { return shield; }
        set { shield = value; }
    }
    // Properties for stamina, strength, endurance, and speed
    public float BaseStamina { get; set; } = 0;
    public float BaseStrength { get; set; } = 0;
    public float BaseEndurance { get; set; } = 0;

    // Berserk effect variables - now using percentages
    private float berserkDamageBoostPercentage = 20; // Damage boost as a percentage
    private float berserkDefenseReductionPercentage = 50; // Defense reduction as a percentage
    private bool isBerserkActive = false; // Is berserk mode active?

    // Stamina-based properties
    public float Stamina => BaseStamina;
    public float MaxHealth => Stamina * StaminaToHealthConversion;

    // Strength-based properties
    public float Strength => BaseStrength;

    // Modified Damage calculation to use percentage boost during berserk
    public float Damage
    {
        get
        {
            float baseDamage = Strength * StrengthToDamageConversion;
            if (isBerserkActive)
            {
                // Apply percentage-based damage boost during berserk
                baseDamage += baseDamage * (berserkDamageBoostPercentage / 100);
            }
            return baseDamage;
        }
    }

    // Endurance-based properties
    [SerializeField] private float _endurance;

    public float Endurance
    {
        get => _endurance;
        set => _endurance = value;
    }

    // Modified DEF calculation to use percentage reduction during berserk
    public float DEF
    {
        get
        {
            float baseDefense = Endurance * EnduranceToDefConversion;
            if (isBerserkActive)
            {
                // Apply percentage-based defense reduction during berserk
                baseDefense -= baseDefense * (berserkDefenseReductionPercentage / 100);
            }
            return baseDefense;
        }
    }

    [SerializeField]
    private float _speed;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

        public void IncreaseShield(float amount)
    {
        Shield += amount;
    }

    public void DecreaseShield(float amount)
    {
        Shield -= amount;
        if (Shield < 0f)
        {
            Shield = 0f;
        }
    }
    // public float DEF => Endurance * EnduranceToDefConversion;

    // Speed-based property

    // Method to handle level changes
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        float previousMaxHealth = damageable.MaxHealth;
        float previouHealth = damageable.Health;
        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        // Update BaseStamina, BaseStrength, and BaseEndurance based on the current level
        float staminaGainFromBuff = 0;
        if (ownedPowerups.IsPowerupActive<Earth_1>())
        {
            Earth_1 e1 = new Earth_1();
            staminaGainFromBuff = BaseStamina * e1.staminaIncrease; // Earth_1 gives 15% stamina gain
            
        }        
        BaseStamina = BaseStamina_PerLevel * currentLevel + BaseStamina_Offset + staminaGainFromBuff;

        float strengthGainFromBuff = 0;
        BaseStrength = BaseStrength_PerLevel * currentLevel + BaseStrength_Offset + strengthGainFromBuff;

        float enduranceGainFromBuff = 0;
        if (ownedPowerups.IsPowerupActive<Metal_1>())
        {
            Metal_1 m1 = new Metal_1();
            enduranceGainFromBuff = BaseEndurance_Offset * m1.enduranceIncrease; // Earth_1 gives 10% endurance gain
            Debug.Log("Earth_1 active, adding stamina gain: " + enduranceGainFromBuff);
        }    
        BaseEndurance = BaseEndurance_PerLevel * currentLevel + BaseEndurance_Offset + enduranceGainFromBuff;

        _endurance = BaseEndurance;

        // Update the Damageable script's health
        if (damageable != null)
        {
            float lossHealth = previousMaxHealth - previouHealth;
            damageable.MaxHealth = (int)MaxHealth;  // Set the new  
            damageable.Health = (int)(MaxHealth - lossHealth); // Set current
        }

        // Update the UI for stamina, health, strength, damage, defense, and speed
        // StaminaText.text = $"Stamina: {Stamina}";
        DamageText.text = $"Damage: {Damage:F1}"; // Display 1 decimal for clarity
        // HealthText.text = $"Max Health: {MaxHealth}";
        DefText.text = $"DEF: {DEF:F1}"; // Display 1 decimal for clarity
        // SpeedText.text = $"Speed: {Speed}"; // Update Speed value on the UI
    }

    // Activate berserk effect
    public void ActivateBerserk()
    {
        isBerserkActive = true;
        UpdateStats();
    }

    // Deactivate berserk effect
    public void DeactivateBerserk()
    {
        isBerserkActive = false;
        UpdateStats();
    }

    // Method to update stats
    private void UpdateStats()
    {
        // This will refresh damage and defense calculations
        DamageText.text = $"Damage: {Damage:F1}"; // Display 1 decimal for clarity
        DefText.text = $"DEF: {DEF:F1}"; // Display 1 decimal for clarity
        // Update other UI elements as needed
    }

    // Start is called before the first frame update
    void Start()
    {
                Shield = 0f; 
        // Find the Damageable component attached to the player
        damageable = GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on the player!");
        }

        // Initialize stats UI at the start (optional)
        OnUpdateLevel(1, 1);  // Initialize at level 1 for example purposes
        _endurance = BaseEndurance;

        _speed = BaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();

        // Optional: Can add additional checks or updates here if needed
    }
}
