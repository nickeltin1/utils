using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Other;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace nickeltin.ObjectPooling
{
    [Serializable]
    public abstract class PoolBase<T> where T : Component
    {
        [SerializeField] protected int _size = 200;
        [SerializeField] protected T _poolObject;
        [SerializeField] protected List<T> _pool = new List<T>();
        [SerializeField] protected List<T> _outOfPoolObjects = new List<T>();
        
        private Action<T> _onItemFirstSpawn;

        private Transform _parent;
        
        public Transform Parent
        {
            get
            {
                if (_parent == null)
                {
                    _parent =  new GameObject(_poolObject.name + "_pool").transform;
                    Object.DontDestroyOnLoad(_parent.gameObject);
                }

                return _parent;
            }
        }

        public int TotalCount { get => _outOfPoolObjects.Count + _pool.Count; }

        public IReadOnlyList<T> Items => _pool;
        public IReadOnlyList<T> OutOfPoolItems => _outOfPoolObjects;
        public Action<T> OnNewItemSpawned => _onItemFirstSpawn;
        

        protected PoolBase(T poolObject, Transform poolParent, int size, Action<T> onItemFirstSpawn)
        {
            this._poolObject = poolObject;
            this._onItemFirstSpawn = onItemFirstSpawn;
            this._parent = poolParent;
            this._size = size;
            if (poolObject.gameObject.activeInHierarchy)
            {
                _onItemFirstSpawn?.Invoke(poolObject);
                this.Add(poolObject, true, false);
            }
        }
        
        public abstract T Get(Transform parent = null);
        
        public void Add(T[] objects, bool forceParent = false, bool keepActive = false)
        {
            for (int i = objects.Length - 1; i >= 0; i--) Add(objects[i], forceParent, keepActive);
        }
        
        public void Add(IList<T> objects, bool forceParent = false, bool keepActive = false)
        {
            for (int i = objects.Count - 1; i >= 0; i--) Add(objects[i], forceParent, keepActive);
        }
        
        public virtual bool Add(T poolObject, bool forceParent = false, bool keepActive = false)
        {
            if (_pool.Contains(poolObject)) return false;
            
            if(forceParent) poolObject.transform.SetParent(Parent);
            if(!keepActive) poolObject.gameObject.SetActive(false);
            _pool.Add(poolObject);
            _outOfPoolObjects.Remove(poolObject);
            
            return true;
        }

        public void Remove(T poolObject)
        {
            if (_pool.Contains(poolObject)) _pool.Remove(poolObject);
            else if (_outOfPoolObjects.Contains(poolObject)) _outOfPoolObjects.Remove(poolObject);
        }

        public void ReturnAllObjectsToPool(bool forceParent = false, bool keepActive = false)
        {
            Add(_outOfPoolObjects, forceParent, keepActive);
        }

        protected T SpawnItem()
        {
            T obj = Object.Instantiate(_poolObject, Parent);
            _onItemFirstSpawn?.Invoke(obj);
            return obj;
        }
    }
}