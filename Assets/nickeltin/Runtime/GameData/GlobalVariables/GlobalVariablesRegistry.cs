using System;
using System.Linq;
using nickeltin.Extensions.Attributes;
using nickeltin.Extensions.Types;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    public abstract class GlobalVariablesRegistry<T> : GlobalVariablesRegistryBase
    {
        [SerializeField, LinkedListsSettings(false, false, runtimeImmutable: true)]
        protected LinkedArrays<string, T> _entries;

        /// <summary> <see cref="int"/> - id of the entry, <see cref="T"/> - new entry value. </summary>
        public event Action<int, T> onEntryChanged;

        public override bool Assigned => _entries.Keys != null;
        public override string[] Keys => _entries.Keys.ToArray();
        public override int Count => _entries.Length;
        
        public T this [int id]
        {
            get => _entries[id];
            set
            {
                _entries[id] = value;
                onEntryChanged?.Invoke(id, value);
            }
        }
    }
}
