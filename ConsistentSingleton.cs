using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Singleton which consists over scenes
    /// </summary>
    /// <typeparam name="T"> Your singleton </typeparam>
    public abstract class ConsistentSingleton<T> : UnconsistentSingleton<T> where T : Component
    {
        /// <summary>
        /// Executing regural singleton pattern with DontDestroyOnLoad to keep object consistent over scenes
        /// </summary>
        protected override void Awake()
        {
            if (instance == null) {
                instance = this as T;
                DontDestroyOnLoad (this);
            } else {
                Destroy(gameObject);
            }
        }
    }
}