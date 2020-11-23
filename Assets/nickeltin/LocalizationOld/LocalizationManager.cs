// using System;
// using System.Collections.Generic;
// using nickeltin.Editor.Attributes;
// using nickeltin.Singletons;
// using UnityEngine;
//
// namespace nickeltin.Localization
// {
//     [CreateAssetMenu(menuName = "Managers/Localization")]
//     public class LocalizationManager : SOSingleton<LocalizationManager>, ILanguagesListProvider
//     {
//         private const string defaultLanguage = "English";
//
//         [ReorderableList] 
//         [SerializeField] private List<string> m_languages = new List<string>{ defaultLanguage };
//
//         [ReorderableList]
//         [SerializeField] private List<Localization> m_localizationSources;
//         
//         [Dropdown("GetLanguagesList")]
//         [SerializeField] private string m_currnetLanguage = defaultLanguage;
//
//         /// <summary>
//         /// (Key1 = language, (Key2 = itemKey, value))
//         /// </summary>
//         private Dictionary<string, Dictionary<string, string>> m_localization;
//
//         public static event Action<string> onLanguageChange;
//         
//         public static List<string> Lanuguages => Instance.GetLanguagesList();
//         public static string CurrentLanguage => Instance.m_currnetLanguage;
//
//         protected override void Initialize()
//         {
//             m_localization = new Dictionary<string, Dictionary<string, string>>();
//             foreach (var lang in m_languages)
//             {
//                 m_localization.Add(lang, new Dictionary<string, string>());
//             }
//             
//             //TODO: localization file loading 
//         }
//
//         private void OnValidate()
//         {
//             if (m_languages.Count <= 0) m_languages.Add(defaultLanguage);
//         }
//         
//         public List<string> GetLanguagesList() => m_languages;
//
//         /// <returns>Returns false if language doesn't exist or its already selected, true is language successfuly changed</returns>
//         public bool ChangeLanguage(string language)
//         {
//             if (m_languages.Contains(language))
//             {
//                 if (!m_currnetLanguage.Equals(language))
//                 {
//                     m_currnetLanguage = language;
//                     onLanguageChange?.Invoke(m_currnetLanguage);
//                     return true;
//                 }
//             }
//             return false;
//         }
//
//         /// <returns>Returns false if language doesn't exist or its already selected, true is language successfuly changed</returns>
//         public static bool ChangeLanguage_Static(string language) => Instance.ChangeLanguage(language);
//         
//         public string GetLocalizedValue(string key)
//         {
//             if(!Exists) FindOrSpawnInstanceAndInitialize();
//
//             if (m_localization[m_currnetLanguage].ContainsKey(key))
//             {
//                 return m_localization[m_currnetLanguage][key];
//             }
//             return null;
//         }
//
//         public static string GetLocalizedValue_Static(string key) => Instance.GetLocalizedValue(key);
//
//     }
// }