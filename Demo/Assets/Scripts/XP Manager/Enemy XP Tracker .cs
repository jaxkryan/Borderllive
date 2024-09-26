using UnityEngine;
using UnityEngine.Events;

public class EnemyXPTracker : MonoBehaviour
{
    private BaseXPTranslation XPTranslationType;
    public UnityEvent<int, int> OnLevelChanged = new UnityEvent<int, int>();

    private BaseXPTranslation XPTranslation;

    // Track the current XP separately in this class
    private int currentXP = 0;

    private void Awake()
    {
        currentXP = PlayerPrefs.GetInt("CurrentXP", 0);
        // Instantiate the XPTranslation based on the assigned type
        if (XPTranslationType == null)
        {
           // Debug.Log("No XP Translation found. Creating new XP Translation Table");
            XPTranslation = ScriptableObject.CreateInstance<EnemyXPTranslation_Table>();
        }
        else
        {
            XPTranslation = ScriptableObject.Instantiate(XPTranslationType);
        }
    }

    public void AddXP(int amount)
    {
        currentXP = PlayerPrefs.GetInt("CurrentXP", 0) + amount;

        // Save the currentXP to PlayerPrefs
        PlayerPrefs.SetInt("CurrentXP", currentXP);
        PlayerPrefs.Save();

        // Handle leveling up based on the amount added
        int previousLevel = XPTranslation.CurrentLevel;
        if (XPTranslation.AddXP(amount))
        {
            OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
        }

        RefreshDisplays();
    }

    public void SetBuffXP(int buffXP)
    {
        // When a buff is picked, set the currentXP based on the buff value
        currentXP = buffXP;
    }

    public void SpawnEnemyWithXP()
    {
        // Transfer the currentXP to the enemy when it spawns
        int xpToGrant = currentXP; // Store the XP to grant

        // Here you would implement the logic to apply xpToGrant to the enemy
        // For example:
        // enemy.SetCurrentXP(xpToGrant);
        //Debug.Log("XP GRANTED SUCESSFULLY: " + xpToGrant);
        // Optionally, set the enemy's level based on the total XP
        XPTranslation.AddXP(xpToGrant);
    }

    public void SetLevel(int level)
    {
        int previousLevel = XPTranslation.CurrentLevel;
        XPTranslation.SetLevel(level);

        if (previousLevel != XPTranslation.CurrentLevel)
        {
            OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
        }

        RefreshDisplays();
    }

    void Start()
    {
        RefreshDisplays();
        OnLevelChanged.Invoke(0, XPTranslation.CurrentLevel);
    }

    void RefreshDisplays()
    {
        // You can implement UI logic here if needed
    }
}
