using System;
using nickeltin.GameData.Events.Types;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public abstract class DataObject<T> : ScriptableObject, IEventBinder<T>
    {
        [TextArea, SerializeField, HideInInspector] private string m_developmentDescription = "";
        [SerializeField] protected bool m_readonly;
        [SerializeField] protected T m_value;

        private event Action<T> m_onValueChanged;

        public bool Readonly => m_readonly;
        
        public virtual T Value
        {
            get => m_value;
            set => TrySetValue(value);
        }

        protected bool TrySetValue(T value)
        {
            if (m_value.Equals(value)) return false;

            if (m_readonly)
            {
                Debug.LogError($"{name} is readonly, setting value is not allowed");
                return false;
            }

            m_value = value;
            
            InvokeUpdate();

            return true;
        }
        
        protected virtual void InvokeUpdate() => m_onValueChanged?.Invoke(Value);

        public void BindEvent(Action<T> onValueChanged) => m_onValueChanged += onValueChanged;

        public void UnbindEvent(Action<T> onValueChanged) => m_onValueChanged -= onValueChanged;

        public static implicit operator T(DataObject<T> reference) => reference.Value;

        public override string ToString() => Value.ToString();
    }
}