using UnityEngine;

namespace Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Instance 
        { 
            get
            {
                if (instance == null)
                    instance = (Instantiate(Resources.Load(typeof(T).Name) as GameObject)).GetComponent<T>();
                return instance;
            } 
        }
        private void Awake()
        {
            if (instance == null) {
                instance = this as T;
                DontDestroyOnLoad (this);
            } else {
                Destroy (gameObject);
            }
        }
    }
}
