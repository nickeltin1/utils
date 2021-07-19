using System;
using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    public abstract class CollectionObject<T> : DataObject<List<T>>
    {
        public int Count => _value.Count;
        
        public override List<T> Value => _value;

        public void AddRange(IEnumerable<T> range, bool invokeUpdate = true)
        {
            _value.AddRange(range);
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void AddItem(T item, bool invokeUpdate = true,  bool uniqueEntry = false)
        {
            if (uniqueEntry)
            {
                if (!_value.Contains(item))
                {
                    _value.Add(item);
                    if(invokeUpdate) InvokeUpdate();
                }
                return;
            }
            
            _value.Add(item); 
            if(invokeUpdate) InvokeUpdate();
        }
        
        public void RemoveItem(T item, bool invokeUpdate = true)
        {
            _value.Remove(item);
            if(invokeUpdate) InvokeUpdate();
        }

        public void Clear(bool invokeUpdate = true)
        {
            _value.Clear();
            if(invokeUpdate) InvokeUpdate();
        }

        public void Sort(Comparison<T> comparison) => _value.Sort(comparison);

        public T this [int i]
        {
            get => _value[i];
            set
            {
                _value[i] = value;
                InvokeUpdate();
            }
        }
    }
}