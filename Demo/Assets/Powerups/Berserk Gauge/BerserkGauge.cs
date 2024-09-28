using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerserkGauge : MonoBehaviour
{
    [SerializeField] Slider berserkSlider; // Reference to the Slider
    [SerializeField] public float maxValue = 100f; // Maximum value of the slider
    private float currentValue = 0f; // Current value of the progress
    private bool isBerserkActive = false; // State of the effect
    public float berserkRegenIncrease { get; set; } =  0f;
    public float berserkRegenDecrease { get; set; } = 0f;

    
    private CharacterStat characterStat; // Reference to CharacterStat

    [SerializeField] GameObject berserkFrameCanva;
    public bool _isBerserkActive => isBerserkActive;
    void Start()
    {
        // Initialize the Slider values
        berserkSlider.minValue = 0;
        berserkSlider.maxValue = maxValue;
        berserkSlider.value = currentValue;

        // Get the CharacterStat component from the character
        characterStat = GetComponent<CharacterStat>();
        berserkFrameCanva.SetActive(false);
    }

    void Update()
    {
        if (isBerserkActive)
        {
            // Calculate the adjusted decrease rate based on the current value
            DecreaseProgress(CalculateDecreaseRate());
        }

        // Update the Slider UI
        berserkSlider.value = currentValue;
    }

    public void IncreaseProgress(float amount)
    {
        currentValue += amount * (1 + berserkRegenIncrease);

        if (isBerserkActive && currentValue >= maxValue)
        {
            currentValue = maxValue;
        }    

        if (currentValue >= maxValue && !isBerserkActive)
        {
            currentValue = maxValue;  // Make sure it's exactly maxValue when berserk activates
            isBerserkActive = true;
            characterStat.ActivateBerserk(); // Activate berserk effects
            if (berserkFrameCanva != null)
            {
                berserkFrameCanva.SetActive(true); // Show the Berserk effect image
            }
        }
    }


    public void DecreaseProgress(float amount)
    {
        currentValue -= amount * (1-berserkRegenDecrease);
        if (currentValue <= 0)
        {
            currentValue = 0;
            if (isBerserkActive)
            {
                isBerserkActive = false;
                characterStat.DeactivateBerserk(); // Deactivate berserk effects
                if (berserkFrameCanva != null)
                {
                    berserkFrameCanva.SetActive(false); // Show the Berserk effect image
                }
            }
        }
    }

    private float CalculateDecreaseRate()
    {
        // Define the decrease rates for each range
        float decreaseRate;

        if (currentValue > 70) // Accelerating drop from 100 to 70
        {
            decreaseRate = 0.30f; // Adjust this value for the speed you desire
        }
        else if (currentValue > 30) // Slowing down drop from 70 to 40
        {
            decreaseRate = 0.20f; // Adjust this value for the speed you desire
        }
        else // Accelerating drop from 40 to 0
        {
            decreaseRate = 0.28f; // Adjust this value for the speed you desire
        }

        return decreaseRate;
    }
}
