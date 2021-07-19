using System.Collections.Generic;
using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
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
        [SerializeField] private List<T> _entries;

        public IReadOnlyList<T> Entries => _entries;
        
        public override bool Register(SaveSystem saveSystem)
        {
            bool value = true;
            
            foreach (var save in _entries)
            {
                if (save != null)
                {
                    save.SaveID.useGUID = true;
                    if(!save.Register(saveSystem)) value = false;
                }
            }

            return value;
        }
        
        public int GetLocalId(T entry) => _entries.IndexOf(entry);
        public T this[int localId]
        {
            get
            {
                if (localId < 0 || localId >= _entries.Count) return null;
                return _entries[localId];
            }
        }
        
#if UNITY_EDITOR
        public static string entries_prop_name => nameof(_entries);
#endif
    }
}