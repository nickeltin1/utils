using UnityEngine;

namespace nickeltin.Localization.Editor
{
    public static class LocalizationEditorHelper
    {
        private const string HelpUrl = "https://hibrahimpenekli.gitbook.io/gametoolkit-localization";
        public const string LocalizationMenu = "Window/Localization/";
        public const string LocalizedElementsSerializedProperty = "m_LocaleItems";
        public const string LocaleLanguageSerializedProperty = "m_Language";
        public const string LocaleValueSerializedProperty = "m_Value";

        public static void OpenHelpUrl()
        {
            Application.OpenURL(HelpUrl);
        }
    }
}