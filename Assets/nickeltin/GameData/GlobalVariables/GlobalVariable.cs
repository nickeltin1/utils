using System;
using UnityEngine;

namespace nickeltin.Experimental.GlobalVariables
{
    [Serializable]
    public sealed class GlobalVariable<T>
    {
        [SerializeField] private GlobalVariablesRegistry<T> m_source;
        [SerializeField] private int m_entryIndex;

        public bool HasSource => m_source != null;
        
        public T Value
        {
            get => m_source[m_entryIndex]; 
            set => m_source[m_entryIndex] = value;
        }

        public static implicit operator T(GlobalVariable<T> obj) => obj.Value;

        public override string ToString() => Value.ToString();
    }
}