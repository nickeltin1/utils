using UnityEngine;

namespace nickeltin.Localization
{
    [AddComponentMenu(ComponentMenuRoot + "Localized Material")]
    public class LocalizedMaterialBehaviour : LocalizedAssetBehaviour
    {
        public Material Material;
        public string PropertyName = "_MainTex";
        public LocalizedTexture LocalizedTexture;

        protected override void UpdateComponentValue()
        {
            if (Material && LocalizedTexture)
            {
                Material.SetTexture(PropertyName, GetValueOrDefault(LocalizedTexture));
            }
        }
    }
}