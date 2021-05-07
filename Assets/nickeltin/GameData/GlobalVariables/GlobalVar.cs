using System;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables
{
    [Serializable]
    public sealed class GlobalVar<T>
    {
        [SerializeField] private GlobalVariablesRegistry<T> m_registry;
        [SerializeField] private int m_entryIndex;
        
        private event Action<T> m_onValueChanged;
        
        public bool HasRegistry => m_registry != null;

        
        public T Value
        {
            get => m_registry[m_entryIndex]; 
            set => m_registry[m_entryIndex] = value;
        }

        public static implicit operator T(GlobalVar<T> obj) => obj.Value;

        public override string ToString() => Value.ToString();
        
        private void OnRegistryUpdate(int id, T value)
        {
            if(id == m_entryIndex) m_onValueChanged?.Invoke(Value);
        }

        public void BindEvent(Action<T> onValueChanged)
        {
            if (HasRegistry)
            {
                if (m_onValueChanged == null) m_registry.onEntryChanged += OnRegistryUpdate;
                m_onValueChanged += onValueChanged;
            }
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if (HasRegistry)
            {
                m_registry.onEntryChanged -= OnRegistryUpdate;
                m_onValueChanged -= onValueChanged;
            }
        }

        public void UnbindAllEvents() => m_onValueChanged = null;
    }
}