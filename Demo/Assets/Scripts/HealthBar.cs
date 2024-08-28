using System;
using System.Collections;
using System.Collections.Generic;
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

        if(player == null )
        {
            Debug.Log("No player found in the scence. Make sure it has tag 'Player'");
        }

        playDamageable = player.GetComponent<Damageable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playDamageable.Health, playDamageable.MaxHealth);
        healthBarText.text = "HP " + playDamageable.Health + " / " + playDamageable.MaxHealth;
    }

    private void OnEnable()
    {
        playDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
