using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.ObjectPooling;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.ObjectPooling
{
    [CustomPropertyDrawer(typeof(PoolObject<>))]
    public class PoolObjectDrawer : GenericObjectDrawer
    {
        private Type _eventType;
        private Type[] _eventParameters;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(PoolObject<>), property, ref _eventType, ref _eventParameters);
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(position, property, label, _eventType, _eventParameters);
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}