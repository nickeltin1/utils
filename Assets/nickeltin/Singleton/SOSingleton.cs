using System.Linq;
using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class SOSingleton<T> : ScriptableObject where T : SOSingleton<T>
    {
        private static readonly string errorMessagePrefix = $"Scriptable Object Singleton of type {typeof(T).Name}";
        
        private static T instance;

        protected static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var objs = Resources.FindObjectsOfTypeAll<T>();
                    
                    if (objs.Length == 0) Debug.LogError(errorMessagePrefix + " not exists in your project, create one");
                    else if (objs.Length > 1) Debug.LogError(errorMessagePrefix + " has more than one instances, delete them for yout project");
                    
                    instance = objs.First();
                }
                
                if (instance == null)
                {
                    instance = CreateInstance<T>();
                    instance.hideFlags = HideFlags.HideAndDontSave;
                }
                
                return instance;
            }
        }
    }
}