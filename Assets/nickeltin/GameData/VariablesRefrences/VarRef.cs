using System;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.GlobalVariables;
using nickeltin.GameData.VariablesRefrences;
using UnityEngine;

namespace nickeltin.GameData.References
{
    [Serializable]
    public class VarRef<T> : VariableReferenceBase
    {
        public enum ReferenceType { Constant, DataObject, GlobalVariable }

        [SerializeField] protected ReferenceType _referenceType;
        [SerializeField] protected T _constantValue;
        [SerializeField] protected DataObject<T> _dataObject; 
        [SerializeField] protected GlobalVar<T> _globalVariable;
        

        public T Value
        {
            get
            {
                if (_referenceType == ReferenceType.DataObject) return _dataObject;
                if (_referenceType == ReferenceType.GlobalVariable) return _globalVariable;

                return _constantValue;
            }
            set
            {
                if (_referenceType == ReferenceType.DataObject) _dataObject.Value = value;
                if (_referenceType == ReferenceType.GlobalVariable) _globalVariable.Value = value;
                else _constantValue = value;
            }
        }
        
        public override object GetValueWithoutType() => Value;
        public override void SetValueWithoutType(object value) => Value = (T) value;

        public static implicit operator T(VarRef<T> reference) => reference.Value;
    }
    
    public abstract class VariableReferenceBase : VariableBase { }
}