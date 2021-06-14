using UnityEngine;

namespace nickeltin.Extensions
{
    public static class ComponentExt
    {
        /// <summary>
        /// Gets component form <paramref name="component" /> if game isn't running, and component isnt null.
        /// /// Use it in <see cref="MonoBehaviour.OnValidate"/>.
        /// </summary>
        public static void Cache<T>(ref T component, GameObject from) where T : Component
        {
            if(component == null) component = from.GetComponent<T>();
        }
        
        /// <summary>
        /// Gets component form <paramref name="from" /> if component isnt null.
        /// Use it in <see cref="MonoBehaviour.OnValidate"/>.
        /// </summary>
        public static void CacheInChildrens<T>(ref T[] components, GameObject from) where T : Component
        {
            if (components == null || components.Length == 0)
            {
                components = from.GetComponentsInChildren<T>(true);
            }
        }
    }
}