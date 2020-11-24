using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace nickeltin.Editor
{
    public static class EditorConsole
    {
        
#if UNITY_EDITOR
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
#endif
        
    }
}
