using System;
using System.Linq;
using nickeltin.Editor.Attributes;
using nickeltin.Editor.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables
{
    public abstract class GlobalVariablesRegistry<T> : GlobalVariablesRegistryBase
    {
        [SerializeField, LinkedListsSettings(false, false, runtimeImmutable: true)] protected LinkedArrays<string, T> m_entries;

        /// <summary> <see cref="int"/> - id of the entry, <see cref="T"/> - new entry value. </summary>
        public event Action<int, T> onEntryChanged;
        
        public override string[] Keys => m_entries.Keys.ToArray();
        public override int Count => m_entries.Length;
        
        public T this [int id]
        {
            get => m_entries[id];
            set
            {
                m_entries[id] = value;
                onEntryChanged?.Invoke(id, value);
            }
        }
    }
}
