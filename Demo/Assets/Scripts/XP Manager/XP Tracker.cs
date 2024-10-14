using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class XPTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CurrentLevelText;  // Text for displaying current level
    [SerializeField] Slider XPSlider;  // Slider for tracking current XP and XP required

    [SerializeField] BaseXPTranslation XPTranslationType;

    [SerializeField] UnityEvent<int, int> OnLevelChanged = new UnityEvent<int, int>();

    private BaseXPTranslation XPTranslation;

    public int CurrentXP => XPTranslation.CurrentXP;
    public int CurrentLevel => XPTranslation.CurrentLevel;

    private void Awake()
    {
        // Check if XPTranslationType is assigned
        if (XPTranslationType == null)
        {
            // Create a new instance of XPTranslation_Table if none is assigned
            XPTranslation = ScriptableObject.CreateInstance<XPTranslation_Table>();
        }
        else
        {
            // Instantiate the provided XPTranslationType
            XPTranslation = ScriptableObject.Instantiate(XPTranslationType);
        }
    }

    public void AddXP(int amount)
    {

        int previousLevel = XPTranslation.CurrentLevel;
        if (XPTranslation.AddXP(amount))
        {
            OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
        }

        RefreshDisplays();
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

    // Start is called before the first frame update
    void Start()
    {
        RefreshDisplays();
        OnLevelChanged.Invoke(0, XPTranslation.CurrentLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RefreshDisplays()
    {
        // Update current level text
        CurrentLevelText.text = $"Level: {XPTranslation.CurrentLevel}";
        
        // Update the slider values based on XP
        if (!XPTranslation.AtLevelCap)
        {
           
            XPSlider.value = XPTranslation.CurrentXP- XPTranslation.GetXPRequiredForCurrentLevel();  // Set max value to XP required for the next level
            XPSlider.maxValue = XPTranslation.GetNextLevelXPRequirement()- XPTranslation.GetXPRequiredForCurrentLevel();  // Set slider's current value to current XP
        }
        else
        {
            XPSlider.maxValue = 1;
            XPSlider.value = 1;
        }
    }
}
