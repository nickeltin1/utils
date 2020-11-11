using Interfaces;
using UnityEngine;

namespace Items
{
    public abstract class WorldItem : MonoBehaviour, IInitializable, IDestructable
    {
        public abstract IInitializable Initialize(IInitializer item);
        public void Destroy() => Destroy(gameObject);
    }
}