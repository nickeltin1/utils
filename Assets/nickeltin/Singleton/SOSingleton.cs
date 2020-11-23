﻿using System.Linq;
using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class SOSingleton<T> : ScriptableObject where T : SOSingleton<T>
    {
        private static readonly string errorMessagePrefix = $"Scriptable Object Singleton of type {typeof(T).Name}";
        
        private static T instance;

        protected static T Instance => FindOrSpawnInstanceAndInitialize();

        [RuntimeInitializeOnLoadMethod]
        protected static T FindOrSpawnInstanceAndInitialize()
        {
            if (!Exists)
            {
                var objs = Resources.FindObjectsOfTypeAll<T>();
                    
                //Debug.Log("Singeltons find " + objs.Length);
                
                if (objs.Length == 0) Debug.LogError(errorMessagePrefix + " not exists in your project, create one");
                else if (objs.Length > 1) Debug.LogError(errorMessagePrefix + " has more than one instances, delete them from your project");
                    
                instance = objs.First();
                    
                if(Exists) instance.Initialize();
            }
                
            if (!Exists && Application.isPlaying)
            {
                instance = CreateInstance<T>();
                instance.hideFlags = HideFlags.HideAndDontSave;
                    
                if(Exists) instance.Initialize();
            }

            return instance;
        }

        public static T GetInstance() => Instance;

        public static bool Exists => instance != null;

        protected abstract void Initialize();
    }
}