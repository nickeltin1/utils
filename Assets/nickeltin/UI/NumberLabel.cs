using nickeltin.GameData.DataObjects;
using TMPro;
using UnityEngine;

namespace nickeltin.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class NumberLabel : MonoBehaviour
    {
        [SerializeField] protected NumberObject m_source;
        [SerializeField] [Range(0, 4)] protected int m_digitsAfterPoint = 2; 
        [SerializeField] protected string m_prefix;
        [SerializeField] protected string m_postfix;
        
        private TMP_Text m_value;

        private void Awake() => m_value = GetComponent<TMP_Text>();


        public void UpdateValue(float newValue)
        {
            m_value.text = m_prefix + newValue.ToString("F" + m_digitsAfterPoint) + m_postfix;
        }
    
        protected virtual void OnEnable()
        {
            if (m_source != null)
            {
                m_source.onValueChanged += UpdateValue;
                UpdateValue(m_source.Value);
            }
        }

        protected virtual void OnDisable()
        {
            if (m_source != null) m_source.onValueChanged -= UpdateValue;
        }
    }
}