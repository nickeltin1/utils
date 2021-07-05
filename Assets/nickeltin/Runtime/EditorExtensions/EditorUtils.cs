using System;
using System.Reflection;
using nickeltin.Runtime.Utility;
using nickeltin.Runtime.Singletons;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Runtime.EditorUtils
{
    public static class EditorUtils
    {
        public static void ClearConsole()
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
#endif
        }
        
#if UNITY_EDITOR

        [MenuItem(MenuPathsUtility.utilsMenu + "DataPath")]
        private static void PresistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        [MenuItem(MenuPathsUtility.utilsMenu +  "Spawn BootStrapper")]
        private static void SpawnSOSBootstraper()
        {
            SOSInitializer sosInitializer = new GameObject("BootStrapper").AddComponent<SOSInitializer>();
            Undo.RegisterCreatedObjectUndo(sosInitializer.gameObject, "BootStrapper created");
            if(sosInitializer.RefreshList() > 0)
            {
                Debug.Log($"BootStrapper spawned, Scriptable Object Singletones from your project added automatically." +
                          $"If some of them is missing add them manualy");
            }
        }
        
#endif
    }
}
