using UnityEngine;
using UnityEngine.UI;

public class BuffSelectionUI : MonoBehaviour
{
    public GameObject buffSelectionPanel;
    public Button[] buffButtons;
    private Powerups[] currentBuffChoices;

    [SerializeField] BuffPool buffPool;
    private OwnedPowerups ownedPowerups;

    void Start()
    {
        buffSelectionPanel.SetActive(false); // Hide the panel initially
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
            Time.timeScale = 0;
            buffSelectionPanel.SetActive(true);

            for (int i = 0; i < buffButtons.Length; i++)
            {
                int index = i; // Cache the index for the button click action
                if (i < currentBuffChoices.Length)
                {
                    buffButtons[i].gameObject.SetActive(true); // Show the button if it has a buff

                    // Get the button
                    Button button = buffButtons[i];

                    // Find the Text components with the names "Name" and "Description"
                    Text nameText = button.transform.Find("Name").GetComponent<Text>();
                    Text descriptionText = button.transform.Find("Description").GetComponent<Text>();

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
            CurrencyManager currency = FindAnyObjectByType<CurrencyManager>();
            currency.AddCurrency(45);
        }

    }

    //vai loz xpAdded bi fix cung
    private static int xpAdded = 0;
    public void SelectBuff(int index)
    {  // Resume the game by setting time scale back to 1
        Time.timeScale = 1;
        Powerups selectedBuff = currentBuffChoices[index];
        ownedPowerups.activePowerups.Add(selectedBuff); // Add selected buff to owned powerups
        ownedPowerups.ActivateAPowerup(selectedBuff); // Activate the selected buff
        buffPool.RemoveBuff(selectedBuff); // Remove the selected buff from the pool
        buffSelectionPanel.SetActive(false); // Hide the panel after selection

        EnemyXPTracker enemyXPTracker = FindAnyObjectByType<EnemyXPTracker>();
        enemyXPTracker.AddXP(selectedBuff.Weight * 20 + xpAdded);
        //Debug.Log(("XP add to enemy" + xpAdded));
        xpAdded += 20;

    }
}
