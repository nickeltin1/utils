using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField, Tooltip("Presists over scenes. If Root scene loaded again will not be re-initialized")] 
        protected bool persistent;
    
        protected static T instance;
        
        public static bool IsOnScene
        {
            get => instance != null;
        }
        
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if(persistent) DontDestroyOnLoad (gameObject);
            }
            else Destroy(gameObject);
        }
        
        
        protected virtual void OnDestroy()
        {
            if (instance == this) instance = null;
        }
    }
}
