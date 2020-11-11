using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectPooling
{
    /// <summary>
    /// Generic object pool
    /// </summary>
    /// <typeparam name="T">PoolObject</typeparam>
    [Serializable]
    public class Pool<T> : PoolBase<T> where T : PoolObject<T>
    {
        public Pool(T poolObject, Transform poolParent = null, int size = 200) : base(poolObject, poolParent, size) { }
        public override T Get()
        {
            if (pool.Count == 0)
            {
                if (outOfPoolObjects.Count >= size)
                {
                    var extractedObject = outOfPoolObjects[outOfPoolObjects.Count - 1];
                    outOfPoolObjects.RemoveAt(outOfPoolObjects.Count - 1);
                    Add(extractedObject);
                }
                else
                {
                    var newObj = Object.Instantiate(poolObject, parent);
                    newObj.Pool = this;
                    Add(newObj);
                }
            }

            var obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);    
            outOfPoolObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public override bool Add(T poolObject, bool forceParent = false)
        {
            if (base.Add(poolObject, forceParent))
            {
                poolObject.Pool = this;
                return true;
            }
            return false;
        }
    }
    
    /// <summary>
    /// Non generic object pool, uses Transforms
    /// </summary>
    [Serializable]
    public class Pool : PoolBase<Transform>
    {
        public Pool(Transform poolObject, Transform poolParent = null, int size = 200) : base(poolObject, poolParent, size) { }
        
        public override Transform Get()
        {
            if (pool.Count == 0)
            {
                if (outOfPoolObjects.Count >= size)
                {
                    var extractedObject = outOfPoolObjects[outOfPoolObjects.Count - 1];
                    outOfPoolObjects.RemoveAt(outOfPoolObjects.Count - 1);
                    Add(extractedObject);
                }
                else Add(Object.Instantiate(poolObject, parent));
            }

            var obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            outOfPoolObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
}