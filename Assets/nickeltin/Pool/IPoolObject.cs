using UnityEngine;

namespace nickeltin.ObjectPooling
{
    public interface IPoolObject<T> where T : Component, IPoolObject<T>
    {
        Pool<T> Pool { get; set; }
    }
}