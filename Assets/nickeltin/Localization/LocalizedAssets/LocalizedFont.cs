using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedFont", menuName = "Localization/Font")]
    public class LocalizedFont : LocalizedAsset<Font>
    {
        [Serializable]
        private class FontLocaleItem : LocaleItem<Font> { };

        [SerializeField]
        private FontLocaleItem[] m_LocaleItems = new FontLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}