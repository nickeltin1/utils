using UnityEngine;

namespace nickeltin.Runtime.Singletons
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
        
        protected virtual void Awake() => Awake_Internal();

        protected bool Awake_Internal()
        {
            if (instance == null)
            {
                instance = this as T;
                if (persistent) DontDestroyOnLoad (gameObject);
                return true;
            }

            Destroy(gameObject);
            return false;
        }
        
        
        protected virtual void OnDestroy() => OnDestroy_Internal();

        protected bool OnDestroy_Internal()
        {
            if (instance == this)
            {
                instance = null;
                return true;
            }

            return false;
        }
    }
}
