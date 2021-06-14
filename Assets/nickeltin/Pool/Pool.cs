﻿using System;
using UnityEngine;
using UnityEngine.Events;
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
        
        public override T Get(Transform parent = null)
        {
            if (_pool.Count == 0)
            {
                if (_outOfPoolObjects.Count >= _size)
                {
                    T extractedObject = _outOfPoolObjects[_outOfPoolObjects.Count - 1];
                    _outOfPoolObjects.RemoveAt(_outOfPoolObjects.Count - 1);
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
            _outOfPoolObjects.Add(obj);
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
        public Pool(Transform poolObject, Transform poolParent = null, int size = 200, 
            Action<Transform> onItemFirstSpawn = null) : base(poolObject, poolParent, size, onItemFirstSpawn) { }
        
        public override Transform Get(Transform parent = null)
        {
            if (_pool.Count == 0)
            {
                if (_outOfPoolObjects.Count >= _size)
                {
                    Transform extractedObject = _outOfPoolObjects[_outOfPoolObjects.Count - 1];
                    _outOfPoolObjects.RemoveAt(_outOfPoolObjects.Count - 1);
                    Add(extractedObject);
                }
                else Add(SpawnItem());
            }

            Transform obj = _pool[_pool.Count - 1];
            _pool.RemoveAt(_pool.Count - 1);
            _outOfPoolObjects.Add(obj);
            obj.gameObject.SetActive(true);
            if(parent != null) obj.transform.SetParent(parent);
            return obj;
        }
    }
}