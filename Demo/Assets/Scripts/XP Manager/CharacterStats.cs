using System.Collections;
using UnityEngine;
using TMPro;

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
    [SerializeField] float BaseEndurance_Offset = 3;    // Fixed offset added to Endurance
    [SerializeField] float EnduranceToDefConversion = 1; // Conversion rate from Endurance to DEF

    // Speed variable
    [SerializeField] float BaseSpeed = 5; // Fixed value for Speed

    // UI elements
    //[SerializeField] TextMeshProUGUI StaminaText;
    //[SerializeField] TextMeshProUGUI HealthText;
    //[SerializeField] TextMeshProUGUI DefText; // UI for DEF (if needed)
    //[SerializeField] TextMeshProUGUI SpeedText; // UI for Speed (if needed)

    // Reference to the Damageable component
    private Damageable damageable;

    private BuffSelectionUI buffSelectionUI;
    // Properties for stamina, strength, endurance, and speed
    public float BaseStamina { get; set; } = 0;
    public float BaseStrength { get; set; } = 0;
    public float BaseEndurance { get; set; } = 0;

    // Stamina-based properties
    public float Stamina => BaseStamina;

    public float MaxHealth => Stamina * StaminaToHealthConversion;

    // Strength-based properties
    public float Strength => BaseStrength;

    public float Damage => Strength * StrengthToDamageConversion;

    // Endurance-based properties
    [SerializeField]
    private float _endurance;

    public float Endurance
    {
        get => _endurance;
        set => _endurance = value;
    }

    //void Start()
    //{
    //   // Set default endurance equal to base
    //}
    public float DEF => Endurance * EnduranceToDefConversion;

    // Speed-based property
    public float Speed => BaseSpeed; // Speed is fixed at 5

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
            damageable.MaxHealth = (int)MaxHealth;  // Set the new MaxHealth
            damageable.Health = (int)MaxHealth;     // Set current health to MaxHealth
        }
        for (int i = previousLevel; i < currentLevel; i++) {
            if (buffSelectionUI == null)
            {
                buffSelectionUI = FindObjectOfType<BuffSelectionUI>();
            }

            if (buffSelectionUI != null)
            {
                buffSelectionUI.ShowBuffChoices();
            }
            else
            {
                Debug.LogError("BuffSelectionUI not found!");
            }
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
        buffSelectionUI = FindObjectOfType<BuffSelectionUI>();
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
