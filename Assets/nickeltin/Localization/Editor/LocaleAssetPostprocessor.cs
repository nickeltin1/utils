using UnityEditor;

namespace nickeltin.Localization.Editor
{
    /// <summary>
    /// Refreshes <see cref="LocalizationWindow"> if opened.
    /// </summary>
    public class LocaleAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                                                   string[] movedAssets, string[] movedFromAssetPaths)
        {
            var localizationWindow = LocalizationWindow.Instance;
            if (localizationWindow)
            {
                localizationWindow.Refresh();
            }
        }
    }
}
