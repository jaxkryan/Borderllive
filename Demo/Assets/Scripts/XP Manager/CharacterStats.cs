using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float BaseEndurance_PerLevel = 2;
    [SerializeField] float BaseEndurance_Offset = 2;
    [SerializeField] float EnduranceToDefConversion = 0.5f;

    // Speed variable
    [SerializeField] float BaseSpeed = 5;

    // Shield-related UI
    [SerializeField] Slider shieldSlider;
    [SerializeField] TextMeshProUGUI shieldText;  // Optional: to display shield amount as text

    // UI elements for Damage and DEF
    [SerializeField] TextMeshProUGUI DamageText;
    [SerializeField] TextMeshProUGUI DefText;

    // Reference to the Damageable component
    private Damageable damageable;

    [SerializeField] private float shield;
    public float Shield
    {
        get { return shield; }
        set
        {
            shield = value;
            // Update the slider and text UI when shield value changes
            shieldSlider.value = shield;
            shieldText.text = $"Shield: {shield:F1}"; // Update shield text (optional)
        }
    }

    // Properties for stamina, strength, endurance, and speed
    public float BaseStamina { get; set; } = 0;
    public float BaseStrength { get; set; } = 0;
    public float BaseEndurance { get; set; } = 0;

    // Berserk effect variables
    private float berserkDamageBoostPercentage = 20;
    private float berserkDefenseReductionPercentage = 50;
    private bool isBerserkActive = false;

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
                baseDefense -= baseDefense * (berserkDefenseReductionPercentage / 100);
            }
            return baseDefense;
        }
    }

    [SerializeField] private float _speed;
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

    // Method to handle level changes
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        float previousMaxHealth = damageable.MaxHealth;
        float previouHealth = damageable.Health;
        OwnedPowerups ownedPowerups = GetComponent<OwnedPowerups>();
        float staminaGainFromBuff = 0;

        if (ownedPowerups.IsPowerupActive<Earth_1>())
        {
            Earth_1 e1 = new Earth_1();
            staminaGainFromBuff = BaseStamina * e1.staminaIncrease; // Earth_1 gives 15% stamina gain
        }

        BaseStamina = BaseStamina_PerLevel * currentLevel + BaseStamina_Offset + staminaGainFromBuff;
        BaseStrength = BaseStrength_PerLevel * currentLevel + BaseStrength_Offset;

        float enduranceGainFromBuff = 0;
        if (ownedPowerups.IsPowerupActive<Metal_1>())
        {
            Metal_1 m1 = new Metal_1();
            enduranceGainFromBuff = BaseEndurance_Offset * m1.enduranceIncrease; // Metal_1 gives 10% endurance gain
            Debug.Log("Metal_1 active, adding endurance gain: " + enduranceGainFromBuff);
        }

        BaseEndurance = BaseEndurance_PerLevel * currentLevel + BaseEndurance_Offset + enduranceGainFromBuff;
        _endurance = BaseEndurance;

        if (damageable != null)
        {
            float lossHealth = previousMaxHealth - previouHealth;
            damageable.MaxHealth = (int)MaxHealth;
            damageable.Health = (int)(MaxHealth - lossHealth);
        }

        shieldSlider.maxValue = MaxHealth;
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
        shieldSlider.value = Shield;
        shieldText.text = $"Shield: {Shield:F1}";
    }

    private void Start()
    {
        Shield = 0f;
        damageable = GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on the player!");
        }

        OnUpdateLevel(1, 1);
        _endurance = BaseEndurance;
        _speed = BaseSpeed;

        shieldSlider.minValue = 0;
        shieldSlider.value = Shield;
        shieldSlider.maxValue = MaxHealth;
    }



    private void Update()
    {
        UpdateStats();

        shieldSlider.value = Shield;
    }
}
