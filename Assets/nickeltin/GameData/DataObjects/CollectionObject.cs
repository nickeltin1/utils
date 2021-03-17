using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class CollectionObject<T> : DataObject<IList<T>>
    {
        [SerializeField] protected List<T> m_collection;

        public override IList<T> Value { get => m_collection;}

        public void AddRange(IEnumerable<T> range, bool invokeUpdate = true)
        {
            m_collection.AddRange(range);
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void AddItem(T item, bool invokeUpdate = true,  bool uniqueEntry = false)
        {
            if (uniqueEntry)
            {
                if (!m_collection.Contains(item))
                {
                    m_collection.Add(item);
                    if(invokeUpdate) InvokeUpdate();
                }
                return;
            }
            
            m_collection.Add(item); 
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void RemoveItem(T item, bool invokeUpdate = true)
        {
            m_collection.Remove(item);
            if(invokeUpdate) InvokeUpdate();
        }

        public void Clear(bool invokeUpdate = true)
        {
            m_collection.Clear();
            if(invokeUpdate) InvokeUpdate();
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