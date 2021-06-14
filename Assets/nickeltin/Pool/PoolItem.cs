using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public class PoolItem<T> : MonoBehaviour, IPoolItem<T> where T : PoolItem<T>
    {
        public Pool<T> Pool { get; set; }

        public void ReturnToPool() => Pool.Add(this as T);

        protected void OnDestroy() => Pool?.Remove(this as T);
    }
}