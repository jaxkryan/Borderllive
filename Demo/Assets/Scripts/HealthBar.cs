using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("No player found in the scence. Make sure it has tag 'Player'");
        }
        playDamageable = player.GetComponent<Damageable>();
    }
    // private void OnEnable()
    // {
    //     playDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    // }

    // private void OnDisable()
    // {
    //     playDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    // }
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
    private void OnEnable()
    {
        playDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
        playDamageable.MaxHealthChanged.AddListener(OnMaxHealthChanged); // Listen for max health changes
    }

    private void OnDisable()
    {
        playDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        playDamageable.MaxHealthChanged.RemoveListener(OnMaxHealthChanged); // Stop listening
    }

    private void OnMaxHealthChanged(int newMaxHealth)
    {
        // Debug.Log("HealthBar received MaxHealthChanged: " + newMaxHealth);
        healthSlider.value = CalculateSliderPercentage(playDamageable.Health, newMaxHealth);
        healthBarText.text = "HP " + playDamageable.Health + " / " + newMaxHealth;
    }


}
