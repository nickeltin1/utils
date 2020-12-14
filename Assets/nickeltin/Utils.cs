using System;
using System.Reflection;
using nickeltin.Singletons;
using UnityEditor;
using UnityEngine;


namespace nickeltin.Editor
{
    public static class Utils
    {
        private const string parentMenu = "Utils/";
        
#if UNITY_EDITOR
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        [MenuItem(parentMenu + "DataPath")]
        private static void PresistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        [MenuItem(parentMenu + "Spawn BootStrapper")]
        private static void SpawnSOSBootstraper()
        {
            SOSInitializer sosInitializer = new GameObject("BootStrapper").AddComponent<SOSInitializer>();
            Undo.RegisterCreatedObjectUndo(sosInitializer.gameObject, "BootStrapper created");
            SOSBase[] sosInProject = Resources.FindObjectsOfTypeAll<SOSBase>();
            if (sosInProject.Length > 0)
            {
                sosInitializer.AddItems(sosInProject);
                Debug.Log($"BootStrapper spawned, Scriptable Object Singletones from your project added automatically." +
                          $"If some of them is missing add them manualy");
            }
        }
#endif
    }
}
