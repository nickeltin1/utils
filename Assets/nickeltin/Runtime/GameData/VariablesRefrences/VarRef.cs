using System;
using nickeltin.Runtime.GameData.DataObjects;
using nickeltin.Runtime.GameData.GlobalVariables;
using UnityEngine;

namespace nickeltin.Runtime.GameData.VariablesRefrences
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

        public override string ToString() => Value.ToString();
    }
    
    public abstract class VariableReferenceBase : VariableBase { }
}