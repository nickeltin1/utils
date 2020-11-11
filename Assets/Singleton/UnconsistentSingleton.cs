using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Generic singleton
    /// Classes inhereted from this class will be singletons
    /// To make sure that object exists in scene put it's prefab into Resources folder and name it after it's class name
    /// </summary>
    /// <typeparam name="T"> Your singleton </typeparam>
    public abstract class UnconsistentSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        /// <summary>
        /// Loads prefab form Resources if it's doesen's exist on scene and return instance
        /// </summary>
        public static T Instance 
        { 
            get
            {
                if (instance == null)
                {
                    instance = (Instantiate(Resources.Load(typeof(T).Name) as GameObject)).GetComponent<T>();
                }
                return instance;
            } 
        }
        
        public static bool IsOnScene
        {
            get => instance != null;
        }
        
        /// <summary>
        /// Executing regural singleton pattern without DontDestroyOnLoad
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null) {
                instance = this as T;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
