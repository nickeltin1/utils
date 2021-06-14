﻿using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public abstract class PoolObject<T> : ScriptableObject where T : Component, IPoolItem<T>
    {
        [SerializeField] private Pool<T> _pool;

        public IReadOnlyList<T> Items => _pool.Items;
        public IReadOnlyList<T> OutOfPoolItems => _pool.OutOfPoolItems;

        public T Get(Transform parent) => _pool.Get(parent);

        public bool Add(T poolObject, bool forceParent = true, bool keepActive = false) =>
            _pool.Add(poolObject, forceParent, keepActive);

        public void ReturnAllObjectsToPool(bool forceParent = true, bool keepActive = false) =>
            _pool.ReturnAllObjectsToPool(forceParent, keepActive);
    }
}