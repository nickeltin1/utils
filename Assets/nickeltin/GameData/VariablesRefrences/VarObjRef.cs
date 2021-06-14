using System;
using Game.Scripts.nickeltin.GameData.VariablesRefrences;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.Events.Types;
using nickeltin.GameData.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.References
{
    [Serializable]
    public sealed class VarObjRef<T> : VariableObjectReferenceBase, IEventBinder<T>
    {
        [Serializable] public enum SourceType { DataObject, GlobalVariable }

        [SerializeField] private SourceType m_sourceType;
        [SerializeField] private DataObject<T> m_dataObjectSource;
        [SerializeField] private GlobalVar<T> m_globalVariableSource;

        public T Value
        {
            get
            {
                if (m_sourceType == SourceType.DataObject) return m_dataObjectSource.Value;
                return m_globalVariableSource.Value;
            }
            set
            {
                if (m_sourceType == SourceType.DataObject) m_dataObjectSource.Value = value;
                else m_globalVariableSource.Value = value;
            }
        }
        
        public bool HasSource
        {
            get
            {
                if (m_sourceType == SourceType.DataObject) return m_dataObjectSource != null;
                return m_globalVariableSource.HasRegistry;
            }
        }
        
        public void BindEvent(Action<T> onValueChanged)
        {
            if (m_sourceType == SourceType.DataObject) m_dataObjectSource.BindEvent(onValueChanged);
            else if (m_sourceType == SourceType.GlobalVariable) m_globalVariableSource.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if (m_sourceType == SourceType.DataObject) m_dataObjectSource.UnbindEvent(onValueChanged);
            else if (m_sourceType == SourceType.GlobalVariable) m_globalVariableSource.UnbindEvent(onValueChanged);
        }
        
        public static implicit operator T(VarObjRef<T> obj) => obj.Value;
        public override object GetValueWithoutType() => Value;

        public override void SetValueWithoutType(object value) => Value = (T) value;

        public override string ToString() => Value.ToString();
    }
    
    public abstract class VariableObjectReferenceBase : VariableBase { }
}
