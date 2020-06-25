using UnityEngine;

namespace Utilities
{
    public abstract class PoolObject<T> : MonoBehaviour where T : PoolObject<T>
    {
        public Pool<T> Pool { get; set; }
    }
}