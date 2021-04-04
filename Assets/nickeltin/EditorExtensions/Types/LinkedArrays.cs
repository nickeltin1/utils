using System;
using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.Editor.Types
{
    [Serializable]
    public class LinkedArrays<T1, T2>
    {
        [SerializeField] private T1[] m_keys;
        [SerializeField] private T2[] m_values;

        public int Length => m_keys.Length;
        
        public IReadOnlyList<T1> Keys => m_keys;
        public IReadOnlyList<T2> Valus => m_values;
        
        public T2 this [int id]
        {
            get => m_values[id]; 
            set => m_values[id] = value;
        }
    }
}