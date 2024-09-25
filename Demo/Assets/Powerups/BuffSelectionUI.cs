using UnityEngine;
using UnityEngine.UI;

public class BuffSelectionUI : MonoBehaviour
{
    public GameObject buffSelectionPanel;
    public Button[] buffButtons;
    private Powerups[] currentBuffChoices;

    private BuffPool buffPool;
    private OwnedPowerups ownedPowerups;

    void Start()
    {
        xpAdded = 230;
        buffSelectionPanel.SetActive(false); // Hide the panel initially
    }
    private void Awake()
    {

        buffPool = FindObjectOfType<BuffPool>();
        ownedPowerups = FindObjectOfType<OwnedPowerups>();
    }

    public void ShowBuffChoices()
    {
        // Pause the game by setting time scale to 0
        Time.timeScale = 0;
        if (buffPool == null)
        {
            Debug.Log("VCL");
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

            for (int i = 0; i < buffButtons.Length; i++)
            {
                int index = i; // Cache the index for the button click action
                if (i < currentBuffChoices.Length)
                {
                    buffButtons[i].gameObject.SetActive(true); // Show the button if it has a buff
                    buffButtons[i].GetComponentInChildren<Text>().text = currentBuffChoices[i].name;
                    buffButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
                    buffButtons[i].onClick.AddListener(() => SelectBuff(index));
                }
                else
                {
                    buffButtons[i].gameObject.SetActive(false); // Hide unused buttons
                }
            }
        }

    }

    private static int xpAdded = 230;
    public void SelectBuff(int index)
    {  // Resume the game by setting time scale back to 1
        Time.timeScale = 1;
        Powerups selectedBuff = currentBuffChoices[index];
        ownedPowerups.activePowerups.Add(selectedBuff); // Add selected buff to owned powerups
        ownedPowerups.ActivateAPowerup(selectedBuff); // Activate the selected buff
        buffPool.RemoveBuff(selectedBuff); // Remove the selected buff from the pool
        buffSelectionPanel.SetActive(false); // Hide the panel after selection

        EnemyXPTracker enemyXPTracker = FindAnyObjectByType<EnemyXPTracker>();
        enemyXPTracker.AddXP(xpAdded);
        Debug.Log(("XP add to enemy" + xpAdded));
        xpAdded += 40;

    }
}
