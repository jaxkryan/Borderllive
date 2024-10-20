using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Dropdown languageDropdown;
    public Slider volumeSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;

    private void Start()
    {

        // Load saved volume and apply it
        float savedVolume = PlayerPrefs.GetFloat("Music", 0.75f); // Default volume is 0.75
        volumeSlider.value = savedVolume;
        SetVolume();

         float savedSfx = PlayerPrefs.GetFloat("Sfx", 0.75f); // Default volume is 0.75
        sfxSlider.value = savedSfx;
        SetSfx();

        // Load fullscreen preference
        bool isFullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false; // Default to fullscreen
        fullscreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;

        int languageIndex = PlayerPrefs.GetInt("Language", 0); // Default quality level is 1 English
        languageDropdown.value = languageIndex;
        SetLanguage(languageIndex);


        // Load quality settings
        int qualityIndex = PlayerPrefs.GetInt("Quality", 2); // Default quality level is 2 (Medium)
        qualityDropdown.value = qualityIndex;
        SetQuality(qualityIndex);

        // Set up resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        // Load and apply saved resolution
        int savedResolutionIndex = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedResolutionIndex);
    }

    public void SetSfx()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Sfx", volume);
        PlayerPrefs.Save();
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        PlayerPrefs.Save();
    }
    public void SetLanguage(int languageIndex)
    {
        ChangeLanguage(languageIndex);
        PlayerPrefs.SetInt("Language", languageIndex);
        PlayerPrefs.Save();
    }
    void ChangeLanguage(int index)
    {
        // Get the selected option
        string selectedLanguage = languageDropdown.options[index].text;
        // Debug.Log(selectedLanguage);
        // Switch locale based on the selected language
        if (selectedLanguage == "English")
        {
            SetLocale("en");
        }
        else if (selectedLanguage == "Tiếng Việt")
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
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index);
        PlayerPrefs.Save();
    }

    private const string PlayerPrefsKeyPrefix = "ItemState_";
    private string[] itemNames = { "item1", "item2", "item3", "item4", "item5", "item6", "item7","item8","item9","item10","sword" }; // Add more if needed

    public void ResetAllData()
    {
        // Store all item states
        Dictionary<string, int> itemStates = new Dictionary<string, int>();

        foreach (var itemName in itemNames)
        {
            string key = PlayerPrefsKeyPrefix + itemName;
            if (PlayerPrefs.HasKey(key))
            {
                itemStates[key] = PlayerPrefs.GetInt(key);
            }
        }
       
        int premiumCurrency = PlayerPrefs.GetInt("PremiumCurrency",0);
        Debug.Log(premiumCurrency);
        PlayerPrefs.DeleteAll();

        // Restore item states
        foreach (var itemState in itemStates)
        {
            PlayerPrefs.SetInt(itemState.Key, itemState.Value);
        }

        // After PlayerPrefs.DeleteAll(), restore the saved premiumCurrency value.
        PlayerPrefs.SetInt("PremiumCurrency", premiumCurrency);

        // Save again to persist restored values
        PlayerPrefs.Save();

    }


}
