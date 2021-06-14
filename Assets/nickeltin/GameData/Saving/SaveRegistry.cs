using System.Collections.Generic;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// All items in registry will be registered to saves database, but will not be LOADED/SAVED.
    /// Can be accessed from database with <see cref="SaveSystem.GetSavedItem{T}"/>.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPathsUtility.savingMenu + nameof(SaveRegistry))]
    public sealed class SaveRegistry : SaveRegistry<SaveableBase> { }

    /// <summary>
    /// Inherit from it to create local database of <see cref="SaveableBase"/> (<see cref="ScriptableObject"/>).
    /// Use <see cref="GetLocalId"/> to get entry id, and Indexer (instance[int localId]) to get item with Id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class SaveRegistry<T> : RegistryItem where T : SaveableBase
    {
        [SerializeField] private List<T> m_entries;

        public IReadOnlyList<T> Entries => m_entries;
        
        public override bool Register(SaveSystem saveSystem)
        {
            bool value = true;
            
            foreach (var save in m_entries)
            {
                if (save != null)
                {
                    save.SaveID.useGUID = true;
                    if(!save.Register(saveSystem)) value = false;
                }
            }

            return value;
        }
        
        public int GetLocalId(T entry) => m_entries.IndexOf(entry);
        public T this[int localId]
        {
            get
            {
                if (localId < 0 || localId >= m_entries.Count) return null;
                return m_entries[localId];
            }
        }
    }
}