using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public interface IPoolItem<T> where T : Component
    {
        Pool<T> Pool { get; set; }
    }
}