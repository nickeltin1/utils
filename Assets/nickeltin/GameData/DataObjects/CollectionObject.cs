using System;
using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class CollectionObject<T> : DataObject<IList<T>>
    {
        [SerializeField] protected List<T> _collection;

        public int Count => _collection.Count;
        
        public override IList<T> Value => _collection;

        public void AddRange(IEnumerable<T> range, bool invokeUpdate = true)
        {
            _collection.AddRange(range);
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void AddItem(T item, bool invokeUpdate = true,  bool uniqueEntry = false)
        {
            if (uniqueEntry)
            {
                if (!_collection.Contains(item))
                {
                    _collection.Add(item);
                    if(invokeUpdate) InvokeUpdate();
                }
                return;
            }
            
            _collection.Add(item); 
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void RemoveItem(T item, bool invokeUpdate = true)
        {
            _collection.Remove(item);
            if(invokeUpdate) InvokeUpdate();
        }

        public void Clear(bool invokeUpdate = true)
        {
            _collection.Clear();
            if(invokeUpdate) InvokeUpdate();
        }

        public void Sort(Comparison<T> comparison) => _collection.Sort(comparison);

        public T this [int i]
        {
            get => _collection[i];
            set
            {
                _collection[i] = value;
                InvokeUpdate();
            }
        }
    }
}