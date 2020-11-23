﻿using System;
using System.Linq;
using UnityEngine;

namespace nickeltin.Localization
{
    public abstract class LocalizedAsset<T> : LocalizedAssetBase where T : class
    {
        /// <summary>
        /// Gets the value type.
        /// </summary>
        public override Type ValueType
        {
            get { return GetValueType(); }
        }

        /// <summary>
        /// Gets the value type.
        /// </summary>
        public static Type GetValueType()
        {
            return typeof(T);
        }

        /// <summary>
        /// Gets the defined locale items of the localized asset with concrete type.
        /// </summary>
        public LocaleItem<T>[] TypedLocaleItems
        {
            get { return (LocaleItem<T>[]) LocaleItems; }
        }

        /// <summary>
        /// Gets localized asset value regarding to <see cref="LocalizationManager.CurrentLanguage"/> if available.
        /// Gets first value of the asset if application is not playing.
        /// </summary>
        /// <seealso cref="Application.isPlaying"/>
        public T Value
        {
            get
            {
                var value = GetLocaleValue(LocalizationManager.CurrentLanguage);


                return value != null ? value : FirstValue;
            }
        }

        /// <summary>
        /// Gets the first locale value of the asset.
        /// </summary>
        public T FirstValue
        {
            get
            {
                var localeItem = TypedLocaleItems.FirstOrDefault();
                return localeItem != null ? localeItem.Value : default(T);
            }
        }

        /// <summary>
        /// Returns the language given is whether exist or not.
        /// </summary>
        public bool HasLocale(SystemLanguage language)
        {
            var localeItem = LocaleItems.FirstOrDefault(x => x.Language == language);
            return localeItem != null;
        }

        /// <summary>
        /// Returns localized text regarding to language given; otherwise, null.
        /// </summary>
        /// <returns>Localized text.</returns>
        public T GetLocaleValue(SystemLanguage language)
        {
            var localeItem = TypedLocaleItems.FirstOrDefault(x => x.Language == language);
            if (localeItem != null)
            {
                return localeItem.Value;
            }

            return null;
        }

        /// <summary>
        /// Returns LocalizedAsset value.
        /// </summary>
        /// <param name="asset">LocalizedAsset</param>
        public static implicit operator T(LocalizedAsset<T> asset) => asset ? asset.Value : default(T);
    }
}