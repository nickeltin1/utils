using System.Linq;
using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class SOSingleton<T> : SOSBase where T : SOSingleton<T>
    {
        private static readonly string errorMessagePrefix = $"Scriptable Object Singleton of type {typeof(T).Name}";
        
        private static T instance;

        protected static T Instance
        {
            get
            {
                FindOrSpawnInstanceAndInitialize();
                return instance;
            }
        }
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        protected static void FindOrSpawnInstanceAndInitialize()
        {
            if (!Exists && !Application.isPlaying)
            {
                var objs = Resources.FindObjectsOfTypeAll<T>();
                    
                //Debug.Log($"Singeltons of type {typeof(T).Name} find: " + objs.Length);
                
                if (objs.Length == 0) Debug.LogError(errorMessagePrefix + " not exists in your project, create one");
                else if (objs.Length > 1) Debug.LogError(errorMessagePrefix + " has more than one instances, delete them from your project");
                    
                objs.First().Initialize();
            }
        }

        public static T GetInstance() => Instance;

        public static bool Exists => instance != null;

        public override bool Initialize()
        {
            if (instance == null)
            {
                instance = this as T;
                return true;
            }

            return false;
        }
    }
}