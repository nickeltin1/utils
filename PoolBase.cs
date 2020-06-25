using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public abstract class PoolBase<T>
    {
        protected Queue<T> pool = new Queue<T>();
        protected T poolObject;
        protected Transform parent;
        
        public abstract T Get();
        public abstract void AddToPool(T obj);
    }
}