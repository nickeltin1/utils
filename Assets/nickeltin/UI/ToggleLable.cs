using nickeltin.GameData.References;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleLable : MonoBehaviour
    {
        [SerializeField] private VarObjRef<bool> m_source;
        [SerializeField] private GameObject m_invertedObject;
        
        private Toggle m_toggle;
        
        private void Awake()
        {
            m_toggle = GetComponent<Toggle>();
            UpdateValue(m_source.Value);
        }

        public void UpdateSource(bool value)
        {
            m_source.Value = value;
            Update_Internal(value);
        }

        private void UpdateValue(bool value)
        {
            m_toggle.isOn = value;
            Update_Internal(value);
        }

        private void Update_Internal(bool state)
        {
            if (m_invertedObject != null) m_invertedObject.SetActive(!state);
        }
        
        private void OnEnable()
        {
            if (m_source.HasSource) m_source.BindEvent(UpdateValue);
        }

        private void OnDisable()
        {
            if (m_source.HasSource) m_source.UnbindEvent(UpdateValue);
        }
    }
}