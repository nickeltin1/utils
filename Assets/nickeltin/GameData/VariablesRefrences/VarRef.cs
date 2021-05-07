using System;
using nickeltin.GameData.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public class VarRef<ValueType> : VariableReferenceBase
    {
        public enum ReferenceType { Constant, DataObject, GlobalVariable }

        [SerializeField] protected ReferenceType m_referenceType;
        [SerializeField] protected ValueType m_constantValue;
        [SerializeField] protected DataObject<ValueType> m_dataObject; 
        [SerializeField] protected GlobalVar<ValueType> m_globalVariable;
        

        public ValueType Value
        {
            get
            {
                if (m_referenceType == ReferenceType.DataObject) return m_dataObject;
                if (m_referenceType == ReferenceType.GlobalVariable) return m_globalVariable;

                return m_constantValue;
            }
            set
            {
                if (m_referenceType == ReferenceType.DataObject) m_dataObject.Value = value;
                if (m_referenceType == ReferenceType.GlobalVariable) m_globalVariable.Value = value;
                else m_constantValue = value;
            }
        }
        
        public override object GetValueWithoutType() => Value;
        public override void SetValueWithoutType(object value)
        {
            m_constantValue = (ValueType) value;
            if (m_dataObject != null) m_dataObject.Value = m_constantValue;
            if (m_globalVariable.HasRegistry) m_globalVariable.Value = m_constantValue;
        }

        public static implicit operator ValueType(VarRef<ValueType> reference) => reference.Value;
    }
    
    public abstract class VariableReferenceBase
    {
        public abstract object GetValueWithoutType();
        public abstract void SetValueWithoutType(object value);
    }
}