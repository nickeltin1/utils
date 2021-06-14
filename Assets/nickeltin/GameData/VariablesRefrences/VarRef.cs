using System;
using Game.Scripts.nickeltin.GameData.Saving;
using Game.Scripts.nickeltin.GameData.VariablesRefrences;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.References
{
    [Serializable]
    public class VarRef<T> : VariableReferenceBase
    {
        public enum ReferenceType { Constant, DataObject, GlobalVariable }

        [SerializeField] protected ReferenceType m_referenceType;
        [SerializeField] protected T m_constantValue;
        [SerializeField] protected DataObject<T> m_dataObject; 
        [SerializeField] protected GlobalVar<T> m_globalVariable;
        

        public T Value
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
        public override void SetValueWithoutType(object value) => Value = (T) value;

        public static implicit operator T(VarRef<T> reference) => reference.Value;
    }
    
    public abstract class VariableReferenceBase : VariableBase { }
}