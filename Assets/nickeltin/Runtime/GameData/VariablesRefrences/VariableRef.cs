using System;
using nickeltin.Runtime.GameData.DataObjects;
using nickeltin.Runtime.GameData.Saving;
using UnityEngine;

namespace nickeltin.Runtime.GameData.VariablesRefrences
{
    [Serializable]
    public struct VariableRef<T> : IValueWithoutTypeProvider
    {
        public enum ReferenceType { Constant, DataObject }

        [SerializeField] private ReferenceType _referenceType;
        [SerializeField] private T _constantValue;
        [SerializeField] private DataObject<T> _dataObject;

        public bool Assignable
        {
            get
            {
                if (_referenceType == ReferenceType.Constant) return true;
                return _dataObject != null;
            }
        }

        public T Value
        {
            get
            {
                if (_referenceType == ReferenceType.DataObject) return _dataObject;
                return _constantValue;
            }
            set
            {
                if (_referenceType == ReferenceType.DataObject) _dataObject.Value = value;
                else _constantValue = value;
            }
        }
        
        public object GetValueWithoutType() => Value;
        public void SetValueWithoutType(object value) => Value = (T) value;

        public VariableRef(T constantValue) : this()
        {
            _constantValue = constantValue;
            _referenceType = ReferenceType.Constant;
        }

        public static implicit operator T(VariableRef<T> reference) => reference.Value;
        public static implicit operator VariableRef<T>(T value) => new VariableRef<T>
        {
            _constantValue = value, _referenceType = ReferenceType.Constant
        };

        public override string ToString() => Value.ToString();
        
#if UNITY_EDITOR
        public static string ref_type_prop_name => nameof(_referenceType);
        public static string const_value_prop_name => nameof(_constantValue);
        public static string data_obj_prop_name => nameof(_dataObject);
#endif
    }
}