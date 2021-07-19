using System;
using JetBrains.Annotations;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.NestedAssets;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [Serializable]
    public abstract class DataObject<T> : DataObjectBase, IEventBinder<T>
    {
        [TextArea, SerializeField, HideInInspector] private string _developmentDescription = "";
        [SerializeField, HideInInspector] protected bool _readonly;
        [SerializeField, HideInInspector] protected T _value;

        private event Action<T> _onValueChanged;

        public bool Readonly => _readonly;
        
        public virtual T Value { get => _value; set => TrySetValue(value); }

        protected virtual bool TrySetValue(T value)
        {
            if (Equals(_value, value)) return false;

            if (_readonly)
            {
                Debug.LogError($"{name} is readonly, setting value is not allowed");
                return false;
            }

            _value = value;
            
            InvokeUpdate();

            return true;
        }
        
        public override void InvokeUpdate() => _onValueChanged?.Invoke(Value);

        public void BindEvent(Action<T> onValueChanged) => _onValueChanged += onValueChanged;

        public void UnbindEvent(Action<T> onValueChanged) => _onValueChanged -= onValueChanged;

        public static implicit operator T(DataObject<T> reference) => reference.Value;

        public override string ToString() => Value.ToString();
        
#if UNITY_EDITOR
        public static string dev_desc_prop_name => nameof(_developmentDescription);
        public static string readonly_prop_name => nameof(_readonly);
        public static string value_prop_name => nameof(_value);
#endif
    }

    public abstract class DataObjectBase : NestedAsset
    {
        public abstract void InvokeUpdate();
    }
}