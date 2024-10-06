using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ListLocalization : MonoBehaviour
{
  public Dropdown languageDropdown;
    
    // Array of Localized Strings for dropdown options
    public LocalizedString[] localizedOptions;

    void Start()
    {
        // Add listener to detect dropdown value change
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Set the dropdown options based on the current language
        UpdateDropdownOptions();

        // Set the dropdown to display the current language
        SetCurrentLanguageInDropdown();
    }

    // Update dropdown options based on the language
    void UpdateDropdownOptions()
    {
        // Ensure the dropdown has the same number of options as localized strings
        languageDropdown.options.Clear();

        foreach (LocalizedString localizedString in localizedOptions)
        {
            // Add a temporary placeholder for the option text
            languageDropdown.options.Add(new Dropdown.OptionData("Loading..."));

            // Subscribe to the string change event to update the text dynamically
            localizedString.StringChanged += (localizedValue) =>
            {
                // Update the option text
                int index = System.Array.IndexOf(localizedOptions, localizedString);
                languageDropdown.options[index].text = localizedValue;

                // Refresh the dropdown to show the updated text
                languageDropdown.RefreshShownValue();
            };
        }
    }

    void SetCurrentLanguageInDropdown()
    {
        string currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;
        if (currentLocale == "en")
            languageDropdown.value = 0; // Assuming 0 is for English
        else if (currentLocale == "vi")
            languageDropdown.value = 1; // Assuming 1 is for Vietnamese
    }

    void OnLanguageChanged(int index)
    {
        string selectedLanguage = languageDropdown.options[index].text;

        // Switch locale based on the selected language (adapt as needed)
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
