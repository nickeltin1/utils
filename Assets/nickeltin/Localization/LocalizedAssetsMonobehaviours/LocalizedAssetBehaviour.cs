using UnityEngine;

namespace nickeltin.Localization
{
    [ExecuteInEditMode]
    public abstract class LocalizedAssetBehaviour : MonoBehaviour
    {
        protected const string ComponentMenuRoot = "Localization/";

        /// <summary>
        /// Component value is updated with the localized asset value.
        /// </summary>
        protected abstract void UpdateComponentValue();

        protected virtual void OnEnable()
        {
            UpdateComponentValue();

            if (Application.isPlaying)
            {
                LocalizationManager.onLocalizationChanged += Localization_LocaleChanged;
            }
        }

        protected virtual void OnDisable()
        {
            if (Application.isPlaying)
            {
                LocalizationManager.onLocalizationChanged -= Localization_LocaleChanged;
            }
        }

        private void OnValidate()
        {
            UpdateComponentValue();
        }

        private void Localization_LocaleChanged(object sender, LocaleChangedEventArgs e)
        {
            UpdateComponentValue();
        }
        
        /// <summary>
        /// Gets the localized value safely.
        /// </summary>
        protected static T GetValueOrDefault<T>(LocalizedAsset<T> localizedAsset) where T : class
        {
            return localizedAsset ? localizedAsset.Value : default(T);
        }
    }
}