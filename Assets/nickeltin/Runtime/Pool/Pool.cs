using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace nickeltin.Runtime.ObjectPooling
{
    /// <summary>
    /// Generic object pool
    /// </summary>
    /// <typeparam name="T">PoolObject</typeparam>
    [Serializable]
    public class Pool<T> : PoolBase<T> where T : Component//, IPoolObject<T>
    {
        public Pool(T poolObject) : 
            base(poolObject, null, DEFAULT_SIZE, null, false) { }

        public Pool(T poolOject, Transform poolParent) : 
            base(poolOject, poolParent, DEFAULT_SIZE, null, false) { }

        public Pool(T poolObject, Transform poolParent, int size = DEFAULT_SIZE, Action<T> onFirstItemSpawn = null,
            bool fillAtCreation = false) :
            base(poolObject, poolParent, size, onFirstItemSpawn, fillAtCreation){ }
        
        
        public override T Get(Transform parent = null)
        {
            if (_pool.Count == 0)
            {
                if (_outOfPoolItems.Count >= _size)
                {
                    T extractedObject = _outOfPoolItems[_outOfPoolItems.Count - 1];
                    _outOfPoolItems.RemoveAt(_outOfPoolItems.Count - 1);
                    Add(extractedObject);
                }
                else
                {
                    T newObj = SpawnItem();
                    AssignPoolToObject(newObj);
                    Add(newObj);
                }
            }

            T obj = _pool[_pool.Count - 1];
            _pool.RemoveAt(_pool.Count - 1);    
            _outOfPoolItems.Add(obj);
            obj.gameObject.SetActive(true);
            if(parent != null) obj.transform.SetParent(parent);
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
            if (obj is IPoolItem<T> p) p.Pool = this;
        }
    }
    
    /// <summary>
    /// Non generic object pool, uses Transforms
    /// </summary>
    [Serializable]
    public class Pool : PoolBase<Transform>
    {
        public Pool(Transform poolObject) : 
            base(poolObject, null, DEFAULT_SIZE, null, false) { }

        public Pool(Transform poolOject, Transform poolParent) : 
            base(poolOject, poolParent, DEFAULT_SIZE, null, false) { }

        public Pool(Transform poolObject, Transform poolParent, int size = DEFAULT_SIZE, Action<Transform> onFirstItemSpawn = null,
            bool fillAtCreation = false) :
            base(poolObject, poolParent, size, onFirstItemSpawn, fillAtCreation){ }

        public override Transform Get(Transform parent = null)
        {
            if (_pool.Count == 0)
            {
                if (_outOfPoolItems.Count >= _size)
                {
                    Transform extractedObject = _outOfPoolItems[_outOfPoolItems.Count - 1];
                    _outOfPoolItems.RemoveAt(_outOfPoolItems.Count - 1);
                    Add(extractedObject);
                }
                else Add(SpawnItem());
            }

            Transform obj = _pool[_pool.Count - 1];
            _pool.RemoveAt(_pool.Count - 1);
            _outOfPoolItems.Add(obj);
            obj.gameObject.SetActive(true);
            if(parent != null) obj.transform.SetParent(parent);
            return obj;
        }
    }
}