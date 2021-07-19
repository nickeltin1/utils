using System.Collections.Generic;

namespace nickeltin.Runtime.GameData.DataObjects
{
    public abstract class RuntimeCollection<T> : CollectionObject<T>
    {
        protected virtual void OnEnable() => _value = new List<T>();
        protected void OnDisable() => _value = new List<T>();
    }
}