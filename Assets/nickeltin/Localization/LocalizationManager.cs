#pragma warning disable

using System;
using System.Collections.Generic;
using System.Linq;
using nickeltin.Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizationManager", menuName = "Localization/Localization Manager")]
    public sealed class LocalizationManager : SOSingleton<LocalizationManager>
    {
        [SerializeField]
        private List<SystemLanguage> m_availableLanguages = new List<SystemLanguage>(1)
        {
            SystemLanguage.English
        };

        [Tooltip("Google Cloud authentication file.")]
        public TextAsset m_googleAuthenticationFile;

        public static TextAsset GoogleAuthenticationFile => Instance.m_googleAuthenticationFile;

        public static List<SystemLanguage> AvailableLanguages => Instance.m_availableLanguages;

        [SerializeField] private SystemLanguage m_currentLanguage = SystemLanguage.English;
        public static SystemLanguage CurrentLanguage => Instance.m_currentLanguage;
        
        public static event EventHandler<LocaleChangedEventArgs> onLocalizationChanged;

        [SerializeField] private UnityEvent<LocaleChangedEventArgs> onLocalizationChangedEvent;
        
        public void ChangeLanguage(LanguageProvider languageProvider) => ChangeLanguage(languageProvider.Enum);

        public void ChangeLanguage(SystemLanguage lang)
        {
            if (m_availableLanguages.Contains(lang))
            {
                if (m_currentLanguage != lang)
                {
                    var previousLanguage = m_currentLanguage;
                    m_currentLanguage = lang;
                    LocalizationChangedInvoke(new LocaleChangedEventArgs(previousLanguage, m_currentLanguage));
                }
            }
            else
            {
                Debug.LogError($"{lang} language doesn't exists in {typeof(LocalizationManager).Name}, please, add it if you want to change to it");
            }
        }

        public static void ChangeLanguage_Static(SystemLanguage lang) => Instance.ChangeLanguage(lang);

        public void SetSystemLanguage() => ChangeLanguage(Application.systemLanguage);
        
        public void SetDefaultLanguage() => ChangeLanguage(AvailableLanguages.FirstOrDefault());

        /// <summary>
        /// Finds all localized assets with type given. Finds all assets in the project if in Editor; otherwise,
        /// finds only that loaded in memory.
        /// </summary>
        /// <returns>Array of specified localized assets.</returns>
        public static T[] FindAllLocalizedAssets<T>() where T : LocalizedAssetBase
        {
#if UNITY_EDITOR
            var guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            var assets = new T[guids.Length];
            for (var i = 0; i < guids.Length; ++i)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                Debug.Assert(assets[i]);
            }

            return assets;
#else
            return Resources.FindObjectsOfTypeAll<T>();
#endif
        }

        /// <summary>
        /// Finds all localized assets.
        /// </summary>
        /// <seealso cref="FindAllLocalizedAssets{T}"/>
        /// <returns>Array of localized assets.</returns>
        public static LocalizedAssetBase[] FindAllLocalizedAssets()
        {
            return FindAllLocalizedAssets<LocalizedAssetBase>();
        }

        /// <summary>
        /// Returns the <see href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">iso-639-1</see> code for the 
        /// specified <paramref name="language"/>.
        /// </summary>
        /// <param name="language">Specified language.</param>
        /// <returns>Two-chararacters iso-639-1 code.</returns>
        public static string GetLanguageCode(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Afrikaans: return "af";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.Basque: return "eu";
                case SystemLanguage.Belarusian: return "be";
                case SystemLanguage.Bulgarian: return "bg";
                case SystemLanguage.Catalan: return "ca";
                case SystemLanguage.Chinese: return "zh";
                case SystemLanguage.Czech: return "cs";
                case SystemLanguage.Danish: return "da";
                case SystemLanguage.Dutch: return "nl";
                case SystemLanguage.English: return "en";
                case SystemLanguage.Estonian: return "et";
                case SystemLanguage.Faroese: return "fo";
                case SystemLanguage.Finnish: return "fi";
                case SystemLanguage.French: return "fr";
                case SystemLanguage.German: return "de";
                case SystemLanguage.Greek: return "el";
                case SystemLanguage.Hebrew: return "he";
                case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Icelandic: return "is";
                case SystemLanguage.Indonesian: return "id";
                case SystemLanguage.Italian: return "it";
                case SystemLanguage.Japanese: return "ja";
                case SystemLanguage.Korean: return "ko";
                case SystemLanguage.Latvian: return "lv";
                case SystemLanguage.Lithuanian: return "lt";
                case SystemLanguage.Norwegian: return "no";
                case SystemLanguage.Polish: return "pl";
                case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Russian: return "ru";
                case SystemLanguage.SerboCroatian: return "hr";
                case SystemLanguage.Slovak: return "sk";
                case SystemLanguage.Slovenian: return "sl";
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Swedish: return "sv";
                case SystemLanguage.Thai: return "th";
                case SystemLanguage.Turkish: return "tr";
                case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi";
                case SystemLanguage.ChineseSimplified: return "zh";
                case SystemLanguage.ChineseTraditional: return "zh";

                default:
                case SystemLanguage.Unknown: return "";
            }
        }
        
        private void LocalizationChangedInvoke(LocaleChangedEventArgs e)
        {
            onLocalizationChanged?.Invoke(this, e);
            onLocalizationChangedEvent.Invoke(e);
        }
    }
    
    public class LocaleChangedEventArgs : EventArgs
    {
        public SystemLanguage PreviousLanguage { get; }
        public SystemLanguage CurrentLanguage { get; }

        public LocaleChangedEventArgs(SystemLanguage previousLanguage, SystemLanguage currentLanguage)
        {
            PreviousLanguage = previousLanguage;
            CurrentLanguage = currentLanguage;
        }
    }
}
