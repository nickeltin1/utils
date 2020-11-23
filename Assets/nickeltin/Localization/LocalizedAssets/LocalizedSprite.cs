using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedSprite", menuName = "Localization/Sprite")]
    public class LocalizedSprite : LocalizedAsset<Sprite>
    {
        [Serializable]
        private class SpriteLocaleItem : LocaleItem<Sprite> { };

        [SerializeField]
        private SpriteLocaleItem[] m_LocaleItems = new SpriteLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
