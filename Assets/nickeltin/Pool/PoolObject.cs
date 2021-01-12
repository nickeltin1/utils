using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public class PoolObject<T> : MonoBehaviour, IPoolObject<T> where T : PoolObject<T>
    {
        public Pool<T> Pool { get; set; }
        
        protected void OnDestroy() => Pool?.Remove(this as T);
    }
}