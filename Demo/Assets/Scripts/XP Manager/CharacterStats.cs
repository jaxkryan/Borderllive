using System.Collections;
using UnityEngine;
using TMPro;

public class CharacterStat : MonoBehaviour
{
    // Stamina variables
    [SerializeField] int BaseStamina_PerLevel = 5;
    [SerializeField] int BaseStamina_Offset = 10;
    [SerializeField] int StaminaToHealthConversion = 10;

    // Strength variables
    [SerializeField] int BaseStrength_PerLevel = 3;
    [SerializeField] int BaseStrength_Offset = 5;
    [SerializeField] int StrengthToDamageConversion = 2;

    // Endurance variables
    [SerializeField] int BaseEndurance_PerLevel = 2;  // Endurance increases per level
    [SerializeField] int BaseEndurance_Offset = 3;    // Fixed offset added to Endurance
    [SerializeField] int EnduranceToDefConversion = 1; // Conversion rate from Endurance to DEF

    // Speed variable
    [SerializeField] int BaseSpeed = 5; // Fixed value for Speed

    // UI elements
    //[SerializeField] TextMeshProUGUI StaminaText;
    //[SerializeField] TextMeshProUGUI HealthText;
    //[SerializeField] TextMeshProUGUI DefText; // UI for DEF (if needed)
    //[SerializeField] TextMeshProUGUI SpeedText; // UI for Speed (if needed)

    // Reference to the Damageable component
    private Damageable damageable;

    // Properties for stamina, strength, endurance, and speed
    public int BaseStamina { get; set; } = 0;
    public int BaseStrength { get; set; } = 0;
    public int BaseEndurance { get; set; } = 0;

    // Stamina-based properties
    public int Stamina => BaseStamina;

    public int MaxHealth => Stamina * StaminaToHealthConversion;

    // Strength-based properties
    public int Strength => BaseStrength;

    public int Damage => Strength * StrengthToDamageConversion;

    // Endurance-based properties
    [SerializeField]
    private int _endurance;

    public int Endurance
    {
        get => _endurance;
        set => _endurance = value;
    }

    //void Start()
    //{
    //   // Set default endurance equal to base
    //}
    public int DEF => Endurance * EnduranceToDefConversion;

    // Speed-based property
    public int Speed => BaseSpeed; // Speed is fixed at 5

    // Method to handle level changes
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        // Update BaseStamina, BaseStrength, and BaseEndurance based on the current level
        BaseStamina = BaseStamina_PerLevel * currentLevel + BaseStamina_Offset;
        BaseStrength = BaseStrength_PerLevel * currentLevel + BaseStrength_Offset;
        BaseEndurance = BaseEndurance_PerLevel * currentLevel + BaseEndurance_Offset;

        // Update the Damageable script's health
        if (damageable != null)
        {
            damageable.MaxHealth = MaxHealth;  // Set the new MaxHealth
            damageable.Health = MaxHealth;     // Set current health to MaxHealth
        }

        // Update the UI for stamina, health, strength, damage, defense, and speed
        // StaminaText.text = $"Stamina: {Stamina}";
       // HealthText.text = $"Max Health: {MaxHealth}";
        //DefText.text = $"DEF: {DEF}"; // Update DEF value on the UI
       // SpeedText.text = $"Speed: {Speed}"; // Update Speed value on the UI
    }

    // Start is called before the first frame update
    void Start()
    {
      
        // Find the Damageable component attached to the player
        damageable = GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on the player!");
        }

        // Initialize stats UI at the start (optional)
        OnUpdateLevel(1, 1);  // Initialize at level 1 for example purposes
        _endurance = BaseEndurance;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Optional: Can add additional checks or updates here if needed
    }
}
