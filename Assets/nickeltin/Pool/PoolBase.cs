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
        [SerializeField] protected List<T> m_pool;
        protected T m_poolObject;
        protected List<T> m_outOfPoolObjects;
        protected int m_size;
        private Action<T> m_onItemFirstSpawn;
        
        public Transform Parent { get; }
        public int TotalCount { get => m_outOfPoolObjects.Count + m_pool.Count; }

        public IReadOnlyList<T> Items => m_pool;
        public IReadOnlyList<T> OutOfPoolItems => m_outOfPoolObjects;
        

        protected PoolBase(T poolObject, Transform poolParent, int size, Action<T> onItemFirstSpawn)
        {
            this.m_pool = new List<T>();
            this.m_outOfPoolObjects = new List<T>();
            this.m_poolObject = poolObject;
            this.m_onItemFirstSpawn = onItemFirstSpawn;
            this.Parent = poolParent == null ? new GameObject(poolObject.name + "_pool").transform : poolParent;
            this.m_size = size;
            if (poolObject.gameObject.activeInHierarchy) this.Add(poolObject, true, false);
        }
        
        public abstract T Get();

        public void Add(IList<T> objects, bool forceParent = false, bool keepActive = false)
        {
            for (int i = objects.Count - 1; i >= 0; i--) Add(objects[i], forceParent, keepActive);
        }
        
        public virtual bool Add(T poolObject, bool forceParent = false, bool keepActive = false)
        {
            if (m_pool.Contains(poolObject)) return false;
            
            if(forceParent) poolObject.transform.SetParent(Parent);
            if(!keepActive) poolObject.gameObject.SetActive(false);
            m_pool.Add(poolObject);
            m_outOfPoolObjects.Remove(poolObject);
            
            return true;
        }

        public void Remove(T poolObject)
        {
            if (m_pool.Contains(poolObject)) m_pool.Remove(poolObject);
            else if (m_outOfPoolObjects.Contains(poolObject)) m_outOfPoolObjects.Remove(poolObject);
        }

        public void ReturnAllObjectsToPool(bool forceParent = false, bool keepActive = false)
        {
            Add(m_outOfPoolObjects, forceParent, keepActive);
        }

        protected T SpawnItem()
        {
            T obj = Object.Instantiate(m_poolObject, Parent);
            m_onItemFirstSpawn?.Invoke(obj);
            return obj;
        }
    }
}