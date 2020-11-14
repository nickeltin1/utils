using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        
        public static bool IsOnScene
        {
            get => instance != null;
        }
        
        /// <summary>
        /// Executing regural singleton pattern without DontDestroyOnLoad
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null) instance = this as T;
            else Destroy(gameObject);
        }
        
        
        /// <summary>
        /// Sets instance to null, if this is instance
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance == this) instance = null;
        }
    }
}
