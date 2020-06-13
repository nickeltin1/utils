using UnityEngine;

namespace Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Generic singleton
        /// Classes inhereted from this class will be singletones
        /// To make sure that object exists is scene put it's prefab into Resources folder and name it after it's class name
        /// </summary>
        private static T instance;
        public static T Instance 
        { 
            get
            {
                //loads prefab form Resources if it's doesen's exist on scene
                if (instance == null)
                    instance = (Instantiate(Resources.Load(typeof(T).Name) as GameObject)).GetComponent<T>();
                return instance;
            } 
        }
        
        //Regural singleton pattern
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
