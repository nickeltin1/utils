using UnityEngine;

namespace nickeltin.Runtime.ObjectPooling
{
    public interface IPoolItem<T> where T : Component
    {
        Pool<T> Pool { get; set; }
    }
}