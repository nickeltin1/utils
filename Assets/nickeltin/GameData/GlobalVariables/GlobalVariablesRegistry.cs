using System.Linq;
using nickeltin.Editor.Attributes;
using nickeltin.Editor.Types;
using UnityEngine;

namespace nickeltin.Experimental.GlobalVariables
{
    public abstract class GlobalVariablesRegistry<T> : VariablesRegistryBase
    {
        [SerializeField, LinkedListsSettings(false, false)] private LinkedLists<string, T> m_entries;
        
        public override string[] Keys => m_entries.Keys.ToArray();
        public override int Count => m_entries.Count;


        public T this [string key] => m_entries[key];
        public T this [int id]
        {
            get => m_entries[id];
            set => m_entries[id] = value;
        }
    }
}
