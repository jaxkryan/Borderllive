using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour
{
    public Dropdown languageDropdown;

    void Start()
    {
        // Add listener to detect dropdown value change
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    void OnLanguageChanged(int index)
    {
        // Get the selected option
        string selectedLanguage = languageDropdown.options[index].text;

        // Switch locale based on the selected language
        if (selectedLanguage == "English")
        {
            SetLocale("en");
        }
        else if (selectedLanguage == "Vietnamese")
        {
            SetLocale("vi-VN");
        }
    }

    void SetLocale(string localeCode)
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales
            .Find(locale => locale.Identifier.Code == localeCode);
        LocalizationSettings.SelectedLocale = selectedLocale;
    }
}
