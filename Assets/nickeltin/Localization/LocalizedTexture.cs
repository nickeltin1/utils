using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedTexture", menuName = "Localization/Texture")]
    public class LocalizedTexture : LocalizedAsset<Texture>
    {
        [Serializable]
        private class TextureLocaleItem : LocaleItem<Texture> { };

        [SerializeField]
        private TextureLocaleItem[] m_LocaleItems = new TextureLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
