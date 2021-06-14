using System;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.Events.Types;
using nickeltin.GameData.GlobalVariables;
using nickeltin.GameData.VariablesRefrences;
using UnityEngine;

namespace nickeltin.GameData.References
{
    [Serializable]
    public sealed class VarObjRef<T> : VariableObjectReferenceBase, IEventBinder<T>
    {
        [Serializable] public enum SourceType { DataObject, GlobalVariable }

        [SerializeField] private SourceType _sourceType;
        [SerializeField] private DataObject<T> _dataObjectSource;
        [SerializeField] private GlobalVar<T> _globalVariableSource;

        public T Value
        {
            get
            {
                if (_sourceType == SourceType.DataObject) return _dataObjectSource.Value;
                return _globalVariableSource.Value;
            }
            set
            {
                if (_sourceType == SourceType.DataObject) _dataObjectSource.Value = value;
                else _globalVariableSource.Value = value;
            }
        }
        
        public bool HasSource
        {
            get
            {
                if (_sourceType == SourceType.DataObject) return _dataObjectSource != null;
                return _globalVariableSource.HasRegistry;
            }
        }
        
        public void BindEvent(Action<T> onValueChanged)
        {
            if (_sourceType == SourceType.DataObject) _dataObjectSource.BindEvent(onValueChanged);
            else if (_sourceType == SourceType.GlobalVariable) _globalVariableSource.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if (_sourceType == SourceType.DataObject) _dataObjectSource.UnbindEvent(onValueChanged);
            else if (_sourceType == SourceType.GlobalVariable) _globalVariableSource.UnbindEvent(onValueChanged);
        }
        
        public static implicit operator T(VarObjRef<T> obj) => obj.Value;
        public override object GetValueWithoutType() => Value;

        public override void SetValueWithoutType(object value) => Value = (T) value;

        public override string ToString() => Value.ToString();
    }
    
    public abstract class VariableObjectReferenceBase : VariableBase { }
}
