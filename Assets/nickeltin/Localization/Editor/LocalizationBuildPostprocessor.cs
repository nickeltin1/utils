using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace nickeltin.Localization.Editor
{
    public class LocalizationBuildPostprocessor
    {
        [PostProcessBuild(9999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                // Continue if any localization info exists.
                var localizations = GetLocalizations();
                if (localizations.Count == 0)
                {
                    return;
                }

                // Get plist.
                var plistPath = pathToBuiltProject + "/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get root of plist/dict.
                var rootDict = plist.root;
                var plistLocalizations = rootDict.CreateArray("CFBundleLocalizations");

                // Add localizations.
                foreach (string locale in localizations)
                {
                    plistLocalizations.AddString(locale);
                    Debug.Log("[LocalizationBuildPostprocessor] Localization added: " + locale);
                }

                // Save all changes.
                File.WriteAllText(plistPath, plist.WriteToString());
            }
#endif
        }

        private static List<string> GetLocalizations()
        {
            var localizations = new List<string>();
            if (LocalizationManager.Exists)
            {
                foreach (var language in LocalizationManager.AvailableLanguages)
                {
                    localizations.Add(LocalizationManager.GetLanguageCode(language));
                }
            }
            return localizations;
        }
    }
}
