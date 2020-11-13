using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace nickeltin.ObjectPooling
{
    [Serializable]
    public abstract class PoolBase<T> where T : Component
    {
        [SerializeField] protected List<T> pool = new List<T>();
        protected T poolObject;
        public Transform parent { get; }
        protected List<T> outOfPoolObjects = new List<T>();
        protected int size;
        public int TotalCount { get => outOfPoolObjects.Count + pool.Count; }


        protected PoolBase(T poolObject, Transform poolParent = null, int size = 200)
        {
            this.poolObject = poolObject;
            this.parent = poolParent == null ? new GameObject(poolObject.name + "_pool").transform : poolParent;
            this.size = size;
            if (poolObject.gameObject.activeInHierarchy) this.Add(poolObject, true);
        }
        
        public abstract T Get();

        public virtual bool Add(T poolObject, [DefaultParameterValue(false)] [Optional] bool forceParent)
        {
            if (pool.Contains(poolObject)) return false;
            
            if(forceParent) poolObject.transform.SetParent(parent);
            poolObject.gameObject.SetActive(false);
            pool.Add(poolObject);
            outOfPoolObjects.Remove(poolObject);
            
            return true;
        }

        public void Remove(T poolObject)
        {
            if (pool.Contains(poolObject)) pool.Remove(poolObject);
            else if (outOfPoolObjects.Contains(poolObject)) outOfPoolObjects.Remove(poolObject);
        }
    }
}