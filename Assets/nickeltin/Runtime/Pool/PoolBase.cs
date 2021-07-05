using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.Runtime.ObjectPooling
{
    [Serializable]
    public abstract class PoolBase<T> where T : Component
    {
        protected const int DEFAULT_SIZE = 200;
        
        [SerializeField] protected int _size = 200;
        [SerializeField] protected T _poolItem;
        [SerializeField] protected List<T> _pool = new List<T>();
        [SerializeField] protected List<T> _outOfPoolItems = new List<T>();
        
        private Action<T> _onItemFirstSpawn;

        private Transform _parent;
        
        public Transform Parent
        {
            get
            {
                if (_parent == null)
                {
                    _parent =  new GameObject(_poolItem.name + "_pool").transform;
                    Object.DontDestroyOnLoad(_parent.gameObject);
                }

                return _parent;
            }
        }

        public int TotalCount { get => _outOfPoolItems.Count + _pool.Count; }

        public IList<T> Items => _pool;
        public IList<T> OutOfPoolItems => _outOfPoolItems;
        public Action<T> OnNewItemSpawned => _onItemFirstSpawn;
        

        protected PoolBase(T poolItem, Transform poolParent, int size, Action<T> onItemFirstSpawn, bool fillAtCreation)
        {
            this._poolItem = poolItem;
            this._onItemFirstSpawn = onItemFirstSpawn;
            this._parent = poolParent;
            this._size = size;
            if (poolItem.gameObject.activeInHierarchy)
            {
                _onItemFirstSpawn?.Invoke(poolItem);
                if (this.Add(poolItem, true, false)) size--;
            }

            if (fillAtCreation)
            {
                for (int i = 0; i < size; i++) Get();
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
            _outOfPoolItems.Remove(poolObject);
            
            return true;
        }

        public bool Remove(T poolItem)
        {
            bool result = false;
            
            if (_pool.Contains(poolItem))
            {
                _pool.Remove(poolItem);
                result = true;
            }
            else if (_outOfPoolItems.Contains(poolItem))
            {
                _outOfPoolItems.Remove(poolItem);
                result = true;
            }

            if (result)
            {
                if (poolItem is IPoolItem<T> item) item.Pool = null;

                return true;
            }
            
            return false;
        }

        public void ReturnAllObjectsToPool(bool forceParent = false, bool keepActive = false)
        {
            Add(_outOfPoolItems, forceParent, keepActive);
        }

        protected T SpawnItem()
        {
            T obj = Object.Instantiate(_poolItem, Parent);
            _onItemFirstSpawn?.Invoke(obj);
            return obj;
        }
    }
}