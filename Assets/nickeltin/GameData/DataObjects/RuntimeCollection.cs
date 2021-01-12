using System.Collections.Generic;

namespace nickeltin.GameData.DataObjects
{
    public abstract class RuntimeCollection<T> : CollectionObject<T>
    {
        protected virtual void OnEnable() => m_collection = new List<T>();
        protected void OnDisable() => m_collection = new List<T>();
    }
}