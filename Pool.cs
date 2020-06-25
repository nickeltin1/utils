using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    /// <summary>
    /// Generic object pool
    /// </summary>
    /// <typeparam name="T">PoolObject</typeparam>
    public class Pool<T> : PoolBase<T> where T : PoolObject<T>
    {
        public Pool(GameObject parent, T poolObject, Transform poolParent = null) 
        {
            this.poolObject = poolObject;
            this.parent = poolParent == null ? new GameObject(poolObject.name + "_pool").transform : poolParent;
            if(poolObject.gameObject.activeInHierarchy) poolObject.transform.SetParent(parent.transform);
        }
        public override T Get()
        {
            if (pool.Count == 0)
            {
                var newObj = Object.Instantiate(poolObject, parent);
                newObj.Pool = this;
                AddToPool(newObj);
            }

            var obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        public override void AddToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    /// <summary>
    /// Non generic object pool, uses regural GameObjects
    /// </summary>
    public class Pool : PoolBase<GameObject>
    {
        public Pool(GameObject parent, GameObject poolObject, Transform poolParent = null)
        {
            this.poolObject = poolObject;
            this.parent = poolParent == null ? new GameObject(poolObject.name + "_pool").transform : poolParent;
            if(poolObject.gameObject.activeInHierarchy) poolObject.transform.SetParent(parent.transform);
        }
        public override GameObject Get()
        {
            if (pool.Count == 0)
            {
                AddToPool(Object.Instantiate(poolObject, parent));
            }

            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        public override void AddToPool(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}