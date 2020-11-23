using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedText", menuName = "Localization/Text")]
    public class LocalizedText : LocalizedAsset<string>
    {
        [Serializable]
        private class TextLocaleItem : LocaleItem<string> { };

        [SerializeField]
        private TextLocaleItem[] m_LocaleItems = new TextLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
