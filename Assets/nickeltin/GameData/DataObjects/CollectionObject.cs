using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class CollectionObject<T> : DataObject<IList<T>>
    {
        [SerializeField] protected List<T> m_collection;

        public override IList<T> Value { get => m_collection;}

        public void AddItem(T item, bool uniqueEntry = false)
        {
            if (uniqueEntry)
            {
                if (!m_collection.Contains(item))
                {
                    m_collection.Add(item);
                    InvokeUpdate();
                }
                return;
            }
            
            m_collection.Add(item); 
            InvokeUpdate();
        }
        
        public void RemoveItem(T item)
        {
            m_collection.Remove(item);
            InvokeUpdate();
        }
        
        public T this [int i]
        {
            get => m_collection[i];
            set
            {
                m_collection[i] = value;
                InvokeUpdate();
            }
        }
    }
}