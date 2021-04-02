using System;
using nickeltin.Experimental.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public class VariableReference<ValueType> : VariableReferenceBase //where ObjectType : DataObject<ValueType>
    {
        public enum ReferenceType
        {
            Constant, DataObject, GlobalVariable
        }

        [SerializeField] protected ReferenceType m_referenceType;
        [SerializeField] protected ValueType m_constantValue;
        [SerializeField] protected DataObject<ValueType> m_dataObject; 
        [SerializeField] protected GlobalVariable<ValueType> m_globalVariable;
        

        public ValueType Value
        {
            get
            {
                if (m_referenceType == ReferenceType.DataObject) return m_dataObject;
                if (m_referenceType == ReferenceType.GlobalVariable) return m_globalVariable;

                return m_constantValue;
            }
        }
        
        public override object GetValueWithoutType() => Value;
        public override void SetValueWithoutType(object value)
        {
            m_constantValue = (ValueType) value;
            if (m_dataObject != null) m_dataObject.Value = m_constantValue;
            if (m_globalVariable.HasSource) m_globalVariable.Value = m_constantValue;
        }

        public static implicit operator ValueType(VariableReference<ValueType> reference)
        {
            return reference.Value;
        }
    }
}