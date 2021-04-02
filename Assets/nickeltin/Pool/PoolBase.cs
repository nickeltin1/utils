using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.ObjectPooling
{
    [Serializable]
    public abstract class PoolBase<T> where T : Component
    {
        [SerializeField] protected List<T> pool;
        protected T poolObject;
        public Transform parent { get; }
        protected List<T> outOfPoolObjects;
        protected int size;
        public int TotalCount { get => outOfPoolObjects.Count + pool.Count; }
        private Action<T> onItemFirstSpawn;

        protected PoolBase(T poolObject, Transform poolParent, int size, Action<T> onItemFirstSpawn)
        {
            this.pool = new List<T>();
            this.outOfPoolObjects = new List<T>();
            this.poolObject = poolObject;
            this.onItemFirstSpawn = onItemFirstSpawn;
            this.parent = poolParent == null ? new GameObject(poolObject.name + "_pool").transform : poolParent;
            this.size = size;
            if (poolObject.gameObject.activeInHierarchy) this.Add(poolObject, true);
        }
        
        public abstract T Get();

        public void Add(IList<T> objects, bool forceParent = false)
        {
            for (int i = objects.Count - 1; i >= 0; i--) Add(objects[i], forceParent);
        }
        
        public virtual bool Add(T poolObject, bool forceParent = false)
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

        public void ReturnAllObjectsToPool(bool forceParent = false)
        {
            Add(outOfPoolObjects, forceParent);
        }

        protected T SpawnItem()
        {
            T obj = Object.Instantiate(poolObject, parent);
            onItemFirstSpawn?.Invoke(obj);
            return obj;
        }
    }
}