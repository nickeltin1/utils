using UnityEngine;

namespace nickeltin.Localization
{
    /// <summary>
    /// Keeps the both asset and the corresponding language.
    /// </summary>
    public class LocaleItem<T> : LocaleItemBase
    {   
        [SerializeField, Tooltip("Locale value.")]
        private T m_Value;

        /// <summary>s
        /// Gets or sets the value of locale item.
        /// </summary>
        public T Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <see cref="LocaleItemBase.ObjectValue"/>
        public override object ObjectValue 
        { 
            get { return m_Value; }
            set { Value = (T) value; }
        }

        public LocaleItem() : base()
        {
        }

        /// <summary>
        /// Creates the locale item with specified language and value.
        /// </summary>
        /// <param name="language">Locale language.</param>
        /// <param name="value">Corresponding locale value.</param>
        public LocaleItem(SystemLanguage language, T value)
        {
            Language = language;
            Value = value;
        }
    }
}
