using TMPro;
using UnityEngine;

namespace nickeltin.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class NumberLabel : MonoBehaviour
    {
        [SerializeField] protected SourceVariant<float> m_source;
        [SerializeField] [Range(0, 4)] protected int m_digitsAfterPoint = 2; 
        [SerializeField] protected string m_prefix;
        [SerializeField] protected string m_postfix;
        
        private TMP_Text m_value;

        private void Awake() => m_value = GetComponent<TMP_Text>();


        public void UpdateValue(float newValue)
        {
            m_value.text = m_prefix + newValue.ToString("F" + m_digitsAfterPoint) + m_postfix;
        }
    
        private void OnEnable()
        {
            if (m_source.CurrentSourcePresented)
            {
                m_source.BindEvent(UpdateValue);
                UpdateValue(m_source.Value);
            }
        }

        private void OnDisable()
        {
            if (m_source.CurrentSourcePresented) m_source.UnbindEvent(UpdateValue);
        }
    }
}