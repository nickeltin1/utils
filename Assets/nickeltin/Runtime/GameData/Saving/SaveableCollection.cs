using System;
using System.Collections.Generic;
using nickeltin.Runtime.GameData.Saving;
using UnityEngine;

namespace GameData.DataObjects
{
    /// <summary>
    /// Inherit form this class to make dynaminc <see cref="ScriptableObject"/> collection, which can save its content.
    /// </summary>
    /// <typeparam name="T">Saveable Type</typeparam>
    public abstract class SaveableCollection<T> : Saveable<string[]> where T : SaveableBase
    {
        [SerializeField] protected List<T> m_defaultCollection;
        [SerializeField] protected List<T> m_collection;

        public event Action onCollectionChange;

        public IList<T> Collection => m_collection;
        
        public void Clear()
        {
            m_collection.Clear();
            onCollectionChange?.Invoke();
        }

        public void AddItem(T item, bool uniqueEntry = false)
        {
            if (uniqueEntry)
            {
                if (!m_collection.Contains(item))
                {
                    m_collection.Add(item);
                    onCollectionChange?.Invoke();
                }
                return;
            }
            
            m_collection.Add(item);
            onCollectionChange?.Invoke();

        }

        public void RemoveItem(T item)
        {
            m_collection.Remove(item);
            onCollectionChange?.Invoke();
        }
        
        protected override void Deserialize(SaveSystem saveSystem, string[] obj)
        {
            File = obj;
            m_collection.Clear();
            for (int i = 0; i < obj.Length; i++)
            {
                if (saveSystem.GetSavedItem<T>(obj[i], out var save))
                {
                    m_collection.Add(save);
                }
                else
                {
                    Debug.LogError($"Item with key {obj[i]}, in object {name}, " +
                                   $"is absent in database. Please add it to saves database in {nameof(SaveSystem)} " +
                                   $"with {nameof(SaveRegistry)}, or like a regural save");
                }
            }
        }

        protected override string[] Serialize()
        {
            string[] obj = new string[m_collection.Count];
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i] = m_collection[i].SaveID;
            }

            return obj;
        }
        
        public override void LoadDefault()
        {
            if(m_collection == null) m_collection = new List<T>();
            m_collection.Clear();
            m_collection.AddRange(m_defaultCollection);
            onCollectionChange?.Invoke();
        }
        
        public T this[int i]
        {
            get => m_collection[i];
            set
            {
                m_collection[i] = value;
                onCollectionChange?.Invoke();
            }
        }
    }
}