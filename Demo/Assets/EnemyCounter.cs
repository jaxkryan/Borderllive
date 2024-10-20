using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class EnemyCounter : MonoBehaviour
{
    public LocalizedString localizedCorpseDefeatedText; // Localized string reference
    [SerializeField] TMP_Text corpseDefeated;
    // [SerializeField] Canvas BuffSelectionUI;
    public EnemySpawnerController enemySpawnerController;
    private void Awake()
    {
        // Set up the localized string to listen for changes
        UpdateCorpseDefeatedText();
        localizedCorpseDefeatedText.StringChanged += UpdateUIText;
        // BuffSelectionUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        // if (Damageable.defeatedEnemyCount % 5 == 0 && Damageable.defeatedEnemyCount > 0 && Time.timeScale == 0)
        // {
        //     if (BuffSelectionUI != null)
        //     {
        //         BuffSelectionUI.gameObject.SetActive(true);
        //         buffSelectionUIScript.ShowBuffChoices();
        //     }
        // }

        // Debug.Log("remain " + enemySpawnerController.remainingEnemy + " time scale " + Time.timeScale);
        UpdateCorpseDefeatedText();
    }
    // Method to update the localized text for defeated enemies
    private void UpdateCorpseDefeatedText()
    {
        // Set the argument based on the defeated enemy count
        localizedCorpseDefeatedText.Arguments = new object[] { Damageable.defeatedEnemyCount };

        // Force refresh the text when the count changes
        localizedCorpseDefeatedText.RefreshString();  // This ensures {0} is updated

    }

    private void UpdateUIText(string updatedText)
    {
        corpseDefeated.text = updatedText; // Update the TMP_Text component with the localized string
    }

    private void OnDestroy()
    {
        localizedCorpseDefeatedText.StringChanged -= UpdateUIText;
    }

    // Call this whenever the enemy is defeated to refresh the text
    public void OnEnemyDefeated()
    {
        UpdateCorpseDefeatedText();
    }
}
