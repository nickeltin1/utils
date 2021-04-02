using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public interface IPoolObject<T> where T : Component
    {
        Pool<T> Pool { get; set; }
    }
}