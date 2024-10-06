using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class GraphicLocalizationDropdown : MonoBehaviour
{
  public Dropdown graphicDropdown;

    // Localized strings for each option
    public LocalizedString lowSetting;
    public LocalizedString mediumSetting;
    public LocalizedString highSetting;
    void Start()
    {
        // Add listener to detect dropdown value change
        graphicDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Set the dropdown options based on the current language
        UpdateDropdownOptions();

        // Set the dropdown to display the current language
        SetCurrentLanguageInDropdown();
    }

    // Update dropdown options based on the language
    void UpdateDropdownOptions()
    {
        lowSetting.StringChanged += (localizedValue) =>
        {
            graphicDropdown.options[0].text = localizedValue;
            graphicDropdown.RefreshShownValue();
        };

        mediumSetting.StringChanged += (localizedValue) =>
        {
            graphicDropdown.options[1].text = localizedValue;
            graphicDropdown.RefreshShownValue();
        };

        highSetting.StringChanged += (localizedValue) =>
        {
            graphicDropdown.options[2].text = localizedValue;
            graphicDropdown.RefreshShownValue();
        };
    }

    void SetCurrentLanguageInDropdown()
    {
        string currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;
        if (currentLocale == "en")
            graphicDropdown.value = 0; // Assuming 0 is for English
        else if (currentLocale == "vi")
            graphicDropdown.value = 1; // Assuming 1 is for Vietnamese
    }

    void OnLanguageChanged(int index)
    {
        string selectedLanguage = graphicDropdown.options[index].text;

        if (selectedLanguage == "English" || selectedLanguage == "Tiếng Anh")
        {
            SetLocale("en");
        }
        else if (selectedLanguage == "Vietnamese" || selectedLanguage == "Tiếng Việt")
        {
            SetLocale("vi");
        }
    }

    void SetLocale(string localeCode)
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales
            .Find(locale => locale.Identifier.Code == localeCode);
        LocalizationSettings.SelectedLocale = selectedLocale;

        // Update dropdown options whenever language changes
        UpdateDropdownOptions();
    }
}
