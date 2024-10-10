using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuffSelectionUI : MonoBehaviour
{
    public GameObject buffSelectionPanel;
    public Button[] buffButtons;
    private Powerups[] currentBuffChoices;

    [SerializeField] BuffPool buffPool;
    private OwnedPowerups ownedPowerups;

    [SerializeField] private TextMeshProUGUI noBuffTextNotify; // Reference to the NoBuffTextNotify

    void Start()
    {
        buffSelectionPanel.SetActive(false); // Hide the panel initially
        noBuffTextNotify.gameObject.SetActive(false); // Hide the no buff notification initially
    }

    private void Awake()
    {
        ownedPowerups = FindObjectOfType<OwnedPowerups>();
    }

    public void ShowBuffChoices()
    {
        // Pause the game by setting time scale to 0
        if (buffPool == null)
        {
            buffPool = FindObjectOfType<BuffPool>();
        }

        if (ownedPowerups == null)
        {
            ownedPowerups = FindObjectOfType<OwnedPowerups>();
        }

        currentBuffChoices = buffPool.GetRandomBuffs(3); // Get 3 random buffs
        if (currentBuffChoices.Length != 0)
        {
            buffSelectionPanel.SetActive(true);

            Time.timeScale = 0;
            for (int i = 0; i < buffButtons.Length; i++)
            {

                int index = i; // Cache the index for the button click action
                if (i < currentBuffChoices.Length)
                {
                    buffButtons[i].gameObject.SetActive(true); // Show the button if it has a buff

                    // Get the button
                    Text nameText = buffButtons[i].transform.Find("Name").GetComponent<Text>();
                    Text descriptionText = buffButtons[i].transform.Find("Description").GetComponent<Text>();

                    // Assign the text values
                    nameText.text = currentBuffChoices[i].nameLocalization.GetLocalizedString();
                    descriptionText.text = currentBuffChoices[i].descriptionLocalization.GetLocalizedString();
                    buffButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
                    buffButtons[i].onClick.AddListener(() => SelectBuff(index));
                }
                else
                {
                    buffButtons[i].gameObject.SetActive(false); // Hide unused buttons
                }
            }
        }
        else
        {
            Time.timeScale = 1;
            CurrencyManager currency = FindAnyObjectByType<CurrencyManager>();
            currency.AddCurrency(45);

            // Show no buff notification
            StartCoroutine(ShowNoBuffNotification());
        }

    }

    // Coroutine to display and hide the NoBuffTextNotify for 1 second
    private IEnumerator ShowNoBuffNotification()
    {
        noBuffTextNotify.gameObject.SetActive(true); // Show the text

        // Capture the initial position and color
        Vector3 startPosition = noBuffTextNotify.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 50, 0); // Move 50 units upward
        float duration = 1f; // Time for animation (1 second)

        // Get the current color of the text
        Color startColor = noBuffTextNotify.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Fade to transparent

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Move upwards and fade out
            noBuffTextNotify.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            noBuffTextNotify.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final position and color are set
        noBuffTextNotify.transform.position = endPosition;
        noBuffTextNotify.color = endColor; // Fully transparent

        yield return new WaitForSeconds(0.5f); // Optional: Add a delay before hiding the text

        noBuffTextNotify.gameObject.SetActive(false); // Hide the text
        noBuffTextNotify.transform.position = startPosition; // Reset the position for next time
        noBuffTextNotify.color = startColor; // Reset the color for next time
    }

    private static int xpAdded = 0;
    public void SelectBuff(int index)
    {
        // Resume the game by setting time scale back to 1
        Time.timeScale = 1;
        Powerups selectedBuff = currentBuffChoices[index];
        ownedPowerups.activePowerups.Add(selectedBuff); // Add selected buff to owned powerups
        ownedPowerups.ActivateAPowerup(selectedBuff); // Activate the selected buff
        buffPool.RemoveBuff(selectedBuff); // Remove the selected buff from the pool
        buffSelectionPanel.SetActive(false); // Hide the panel after selection

        EnemyXPTracker enemyXPTracker = FindAnyObjectByType<EnemyXPTracker>();
        enemyXPTracker.AddXP(selectedBuff.Weight * 10 + xpAdded);
        xpAdded += 20;
    }
}
