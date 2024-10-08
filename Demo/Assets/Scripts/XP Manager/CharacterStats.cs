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
    //[SerializeField] TextMeshProUGUI DamageText;
    //[SerializeField] TextMeshProUGUI DefText;

    // Reference to the Damageable component
    private Damageable damageable;

    // Shield property
    [SerializeField] private float shield;
    public float Shield
    {
        get { return shield; }
        set
        {
            shield = value;
            shieldSlider.value = shield;
            shieldText.text = $"Shield: {shield:F1}";
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

    // Permanent increase fields
    private float permanentStaminaIncrease;
    private float permanentStrengthIncrease;
    private float permanentEnduranceIncrease;

    // Toggle for permanent buffs
    public bool applyPermanentBuffs = true;

    // Save keys for PlayerPrefs
    private const string StaminaIncreaseKey = "StaminaIncrease";
    private const string StrengthIncreaseKey = "StrengthIncrease";
    private const string EnduranceIncreaseKey = "EnduranceIncrease";
    private const string ApplyBuffsKey = "ApplyBuffs";

    // Stamina-based properties
    public float Stamina => BaseStamina + (applyPermanentBuffs ? permanentStaminaIncrease : 0);
    public float MaxHealth => Stamina * StaminaToHealthConversion;

    // Strength-based properties
    public float Strength => BaseStrength + (applyPermanentBuffs ? permanentStrengthIncrease : 0);

    // Damage calculation with berserk effect
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
        get => _endurance + (applyPermanentBuffs ? permanentEnduranceIncrease : 0);
        set => _endurance = value;
    }

    // DEF calculation with berserk effect
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


    private static int levelupCount = 0;

    // Handle level updates and apply permanent increases if buffs are active
    public void OnUpdateLevel(int previousLevel, int currentLevel)
    {
        float previousMaxHealth = damageable.MaxHealth;
        float previousHealth = damageable.Health;

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
        }

        BaseEndurance = BaseEndurance_PerLevel * currentLevel + BaseEndurance_Offset + enduranceGainFromBuff;
        _endurance = BaseEndurance;

        levelupCount += currentLevel - previousLevel;

        // Apply permanent increases every 10 levels if buffs are enabled
        if (levelupCount % 10 == 0 && applyPermanentBuffs)
        {
            permanentStaminaIncrease += 0.09f;
            permanentStrengthIncrease += 0.04f;
            permanentEnduranceIncrease += 0.04f;

            // Save to PlayerPrefs
            PlayerPrefs.SetFloat(StaminaIncreaseKey, permanentStaminaIncrease);
            PlayerPrefs.SetFloat(StrengthIncreaseKey, permanentStrengthIncrease);
            PlayerPrefs.SetFloat(EnduranceIncreaseKey, permanentEnduranceIncrease);
        }

        if (damageable != null)
        {
            float lostHealth = previousMaxHealth - previousHealth;
            damageable.MaxHealth = (int)MaxHealth;
            damageable.Health = (int)(MaxHealth - lostHealth);
        }

        shieldSlider.maxValue = MaxHealth;
    }

    // Method to reset the permanent buffs
    public void ResetPermanentBuffs()
    {
        permanentStaminaIncrease = 0f;
        permanentStrengthIncrease = 0f;
        permanentEnduranceIncrease = 0f;

        // Clear PlayerPrefs
        PlayerPrefs.DeleteKey(StaminaIncreaseKey);
        PlayerPrefs.DeleteKey(StrengthIncreaseKey);
        PlayerPrefs.DeleteKey(EnduranceIncreaseKey);
        PlayerPrefs.Save();

        // Recalculate the stats to remove the buffs
        UpdateStats();

        Debug.Log("Permanent buffs have been reset.");
    }

    private void LoadPermanentIncreases()
    {
        permanentStaminaIncrease = PlayerPrefs.GetFloat(StaminaIncreaseKey, 0);
        permanentStrengthIncrease = PlayerPrefs.GetFloat(StrengthIncreaseKey, 0);
        permanentEnduranceIncrease = PlayerPrefs.GetFloat(EnduranceIncreaseKey, 0);

        // Set buffs to disabled by default (use 0 as the default value)
        applyPermanentBuffs = PlayerPrefs.GetInt(ApplyBuffsKey, 0) == 1;
    }


    // Toggle for applying permanent buffs
    public void TogglePermanentBuffs(bool value)
    {
        applyPermanentBuffs = value;
        SaveApplyBuffsOption(value);
        UpdateStats();
    }

    private void SaveApplyBuffsOption(bool value)
    {
        PlayerPrefs.SetInt(ApplyBuffsKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Method to update stats and apply buffs
    private void UpdateStats()
    {
        // Apply changes to stamina, strength, endurance
        shieldSlider.value = Shield;
        shieldText.text = $"Shield: {Shield:F1}";

        // Update UI for Damage and DEF
        //DamageText.text = $"Damage: {Damage:F1}";
       // DefText.text = $"DEF: {DEF:F1}";

        // Recalculate the player's max health based on the updated stamina
        if (damageable != null)
        {
            damageable.MaxHealth = (int)MaxHealth;
        }
    }

    private void Start()
    {
        Shield = 0f;
        damageable = GetComponent<Damageable>();
        if (damageable == null)
        {
            Debug.LogError("Damageable component not found on the player!");
        }

        LoadPermanentIncreases();
        OnUpdateLevel(1, 1);  // Set initial level stats
        _endurance = BaseEndurance;
        _speed = BaseSpeed;

        shieldSlider.minValue = 0;
        shieldSlider.value = Shield;
        shieldSlider.maxValue = MaxHealth;
        levelupCount = 0;
    }

    private void Update()
    {
        UpdateStats();
        shieldSlider.value = Shield;
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
}

