using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Types
{
    [Serializable]
    public class LinkedLists<T1, T2>
    {
        [SerializeField] private List<T1> m_keys = new List<T1>();
        [SerializeField] private List<T2> m_values = new List<T2>();

        public int Count => m_keys.Count;
        
        public IReadOnlyList<T1> Keys => m_keys;
        public IReadOnlyList<T2> Valus => m_values;
        
        public T2 this [T1 key] => m_values[m_keys.IndexOf(key)];
        public T2 this [int id]
        {
            get => m_values[id]; 
            set => m_values[id] = value;
        }
    }
}