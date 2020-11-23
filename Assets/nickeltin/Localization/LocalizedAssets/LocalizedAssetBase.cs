using System;
using UnityEngine;

namespace nickeltin.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LocalizedAssetBase : ScriptableObject
    {
        /// <summary>
        /// Gets the defined locale items of the localized asset.
        /// </summary>
        public abstract LocaleItemBase[] LocaleItems { get; }

        /// <summary>
        /// Gets the value type.
        /// </summary>
        public abstract Type ValueType { get; }
    }
}