using nickeltin.Localization;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Localization/Localization Prefs")]
public class LocalizationPrefs : MonoBehaviour
{
    [SerializeField, Tooltip("PlayerPrefs key to keep language preference.")]
    private string m_PrefKey = "GameLanguage";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadLanguage();
        LocalizationManager.onLocalizationChanged += LocalizationOnOnLocalizationChanged;
    }

    private void OnDestroy()
    {
        LocalizationManager.onLocalizationChanged -= LocalizationOnOnLocalizationChanged;
    }

    private void LocalizationOnOnLocalizationChanged(object sender, LocaleChangedEventArgs e)
    {
        SaveLanguage();
    }

    private void LoadLanguage()
    {
        // Set previously saved language if available.
        var savedLanguage = GetSavedLanguage();
        if (savedLanguage != SystemLanguage.Unknown)
        {
            LocalizationManager.ChangeLanguage_Static(savedLanguage);
        }
    }

    private void SaveLanguage()
    {
        PlayerPrefs.SetInt(m_PrefKey, (int) LocalizationManager.CurrentLanguage);
        PlayerPrefs.Save();
    }

    private SystemLanguage GetSavedLanguage()
    {
        if (PlayerPrefs.HasKey(m_PrefKey))
        {
            var languageValue = PlayerPrefs.GetInt(m_PrefKey, -1);
            if (languageValue >= 0)
            {
                return (SystemLanguage) languageValue;
            }
        }

        return SystemLanguage.Unknown;
    }
}