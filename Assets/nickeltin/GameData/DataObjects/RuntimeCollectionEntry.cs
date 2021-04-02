using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class RuntimeCollectionEntry<T> : MonoBehaviour where T : RuntimeCollectionEntry<T>
    {
        [SerializeField] protected RuntimeCollection<T> m_collection;

        private void OnEnable()
        {
            if (m_collection != null) m_collection.AddItem(this as T);
        }

        private void OnDisable()
        {
            if (m_collection != null) m_collection.RemoveItem(this as T);
        }
    }
}