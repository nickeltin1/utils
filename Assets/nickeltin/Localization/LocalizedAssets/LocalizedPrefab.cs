using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedPrefab", menuName = "Localization/Prefab")]
    public class LocalizedPrefab : LocalizedAsset<GameObject>
    {
        [Serializable]
        private class PrefabLocaleItem : LocaleItem<GameObject> { };

        [SerializeField]
        private PrefabLocaleItem[] m_LocaleItems = new PrefabLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}