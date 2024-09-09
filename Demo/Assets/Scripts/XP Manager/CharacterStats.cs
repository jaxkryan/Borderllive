using System.Collections;
using System.Collections.Generic;
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

    // UI elements
    [SerializeField] TextMeshProUGUI StaminaText;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI StrengthText;
    [SerializeField] TextMeshProUGUI DamageText;

    // Reference to the Damageable component
    private Damageable damageable;

    // Properties for stamina and strength
    public int BaseStamina { get; protected set; } = 0;
    public int BaseStrength { get; protected set; } = 0;

    // Stamina-based properties
    public int Stamina
    {
        get { return BaseStamina; }
    }

    public int MaxHealth
    {
        get { return Stamina * StaminaToHealthConversion; }
    }

    // Strength-based properties
    public int Strength
    {
        get { return BaseStrength; }
    }

    public int Damage
    {
        get { return Strength * StrengthToDamageConversion; }
    }

    // Method to handle level changes
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        // Update BaseStamina and BaseStrength based on the current level
        BaseStamina = BaseStamina_PerLevel * currentLevel + BaseStamina_Offset;
        BaseStrength = BaseStrength_PerLevel * currentLevel + BaseStrength_Offset;

        // Update the Damageable script's health
        if (damageable != null)
        {
            damageable.MaxHealth = MaxHealth;  // Set the new MaxHealth
            damageable.Health = MaxHealth;     // Set current health to MaxHealth
        }

        // Update the UI for stamina, health, strength, and damage
        StaminaText.text = $"Stamina: {Stamina}";
        HealthText.text = $"Max Health: {MaxHealth}";
        StrengthText.text = $"Strength: {Strength}";
        DamageText.text = $"Damage: {Damage}";
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
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: Can add additional checks or updates here if needed
    }
}
