using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class RuntimeCollectionEntry<T> : MonoBehaviour where T : RuntimeCollectionEntry<T>
    {
        [SerializeField] protected RuntimeCollection<T> _collection;

        private void OnEnable()
        {
            if (_collection != null) _collection.AddItem(this as T);
        }

        private void OnDisable()
        {
            if (_collection != null) _collection.RemoveItem(this as T);
        }
    }
}