using System;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.GlobalVariables;
using UnityEngine;

namespace nickeltin.UI
{
    [Serializable]
    public class SourceVariant<T>
    {
        [Serializable] public enum SourceType { DataObject, GlobalVariable }

        [SerializeField] private SourceType m_sourceType;
        [SerializeField] private DataObject<T> m_dataObjectSource;
        [SerializeField] private GlobalVariable<T> m_globalVariableSource;

        public T Value
        {
            get
            {
                if (m_sourceType == SourceType.DataObject) return m_dataObjectSource.Value;
                return m_globalVariableSource.Value;
            }
        }
        
        public bool CurrentSourcePresented
        {
            get
            {
                if (m_sourceType == SourceType.DataObject) return m_dataObjectSource != null;
                return m_globalVariableSource.HasRegistry;
            }
        }
        
        public void BindEvent(Action<T> onValueChanged)
        {
            if (m_sourceType == SourceType.DataObject) m_dataObjectSource.onValueChanged += onValueChanged;
            else if (m_sourceType == SourceType.GlobalVariable) m_globalVariableSource.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if (m_sourceType == SourceType.DataObject) m_dataObjectSource.onValueChanged -= onValueChanged;
            else if (m_sourceType == SourceType.GlobalVariable) m_globalVariableSource.UnbindEvent(onValueChanged);
        }
    }
}