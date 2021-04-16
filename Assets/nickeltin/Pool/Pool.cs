using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.ObjectPooling
{
    /// <summary>
    /// Generic object pool
    /// </summary>
    /// <typeparam name="T">PoolObject</typeparam>
    [Serializable]
    public class Pool<T> : PoolBase<T> where T : Component//, IPoolObject<T>
    {
        public Pool(T poolObject, Transform poolParent = null, int size = 200, Action<T> onItemFirstSpawn = null ) : 
            base(poolObject, poolParent, size, onItemFirstSpawn) { }
        
        public override T Get()
        {
            if (m_pool.Count == 0)
            {
                if (m_outOfPoolObjects.Count >= m_size)
                {
                    T extractedObject = m_outOfPoolObjects[m_outOfPoolObjects.Count - 1];
                    m_outOfPoolObjects.RemoveAt(m_outOfPoolObjects.Count - 1);
                    Add(extractedObject);
                }
                else
                {
                    T newObj = SpawnItem();
                    AssignPoolToObject(newObj);
                    Add(newObj);
                }
            }

            T obj = m_pool[m_pool.Count - 1];
            m_pool.RemoveAt(m_pool.Count - 1);    
            m_outOfPoolObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public override bool Add(T poolObject, bool forceParent = false,  bool keepActive = false)
        {
            if (base.Add(poolObject, forceParent, keepActive))
            {
                AssignPoolToObject(poolObject);
                return true;
            }
            return false;
        }

        private void AssignPoolToObject(T obj)
        {
            if (obj is IPoolObject<T> p) p.Pool = this;
        }
    }
    
    /// <summary>
    /// Non generic object pool, uses Transforms
    /// </summary>
    [Serializable]
    public class Pool : PoolBase<Transform>
    {
        public Pool(Transform poolObject, Transform poolParent = null, int size = 200, 
            Action<Transform> onItemFirstSpawn = null) : base(poolObject, poolParent, size, onItemFirstSpawn) { }
        
        public override Transform Get()
        {
            if (m_pool.Count == 0)
            {
                if (m_outOfPoolObjects.Count >= m_size)
                {
                    Transform extractedObject = m_outOfPoolObjects[m_outOfPoolObjects.Count - 1];
                    m_outOfPoolObjects.RemoveAt(m_outOfPoolObjects.Count - 1);
                    Add(extractedObject);
                }
                else Add(SpawnItem());
            }

            Transform obj = m_pool[m_pool.Count - 1];
            m_pool.RemoveAt(m_pool.Count - 1);
            m_outOfPoolObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
}