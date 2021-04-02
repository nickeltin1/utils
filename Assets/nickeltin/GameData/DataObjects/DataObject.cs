using System;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public abstract class DataObject<T> : ScriptableObject
    {
        [TextArea, SerializeField] private string m_developmentDescription = "";
        [Space]
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
        [Button("Invoke Update", EButtonEnableMode.Playmode)]
        public virtual void InvokeUpdate() => onValueChanged?.Invoke(Value);

        public static implicit operator T(DataObject<T> reference) => reference.Value;

        public override string ToString() => Value.ToString();
    }
}