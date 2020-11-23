using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedTextAsset", menuName = "Localization/Text Asset")]
    public class LocalizedTextAsset : LocalizedAsset<TextAsset>
    {
        [Serializable]
        private class TextAssetLocaleItem : LocaleItem<TextAsset> { };

        [SerializeField]
        private TextAssetLocaleItem[] m_LocaleItems = new TextAssetLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
