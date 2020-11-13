using System;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class DataObject<T> : ScriptableObject
    {
        [SerializeField] protected T m_value;

        public virtual T Value
        {
            get => m_value;
            set
            {
                m_value = value;
                InvokeUpdate();   
            }
        }

        public event Action<T> onValueChanged;
        protected virtual void InvokeUpdate() => onValueChanged?.Invoke(m_value);
    }
}