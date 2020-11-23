using System;
using UnityEngine;

namespace nickeltin.Localization
{
    [AddComponentMenu(ComponentMenuRoot + "Localized Text Asset")]
    public class LocalizedTextAssetBehaviour : LocalizedGenericAssetBehaviour<LocalizedTextAsset, TextAsset>
    {
        protected override Type GetValueType()
        {
            return typeof(string);
        }

        protected override object GetLocalizedValue()
        {
            return m_LocalizedAsset ? m_LocalizedAsset.Value.text : null;
        }
    }
}