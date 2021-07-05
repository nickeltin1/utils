using nickeltin.Runtime.GameData.DataObjects;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.Runtime.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleLable : ValueObserverBase
    {
        [SerializeField] private DataObject<bool> m_source;
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
            InvokeUnityEvent();
        }
        
        private void OnEnable() => m_source.BindEvent(UpdateValue);

        private void OnDisable() => m_source.UnbindEvent(UpdateValue);
    }
}