namespace nickeltin.Localization.Editor
{
    /// <summary>
    /// Refreshes <see cref="LocalizationWindow"/> if opened.
    /// </summary>
    public class LocaleAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string[] paths)
        {
            var localizationWindow = LocalizationWindow.Instance;
            if (localizationWindow)
            {
                /// This will reset assets <see cref="AssetTreeViewItem.IsDirty"/> flag.
                localizationWindow.Refresh();
            }
            return paths;
        }
    }
}
