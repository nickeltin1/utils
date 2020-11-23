using UnityEngine;

namespace nickeltin.Localization
{
    public abstract class LocaleItemBase
    {
        [SerializeField, Tooltip("Locale language.")]
        private SystemLanguage m_Language = SystemLanguage.English;

        /// <summary>
        /// Gets or sets the language of the locale.
        /// </summary>
        public SystemLanguage Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }

        /// <summary>
        /// Gets the value of the locale.
        /// </summary>
        public abstract object ObjectValue { get; set; }
    }
}