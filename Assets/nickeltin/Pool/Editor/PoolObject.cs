using System;
using nickeltin.Editor.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace nickeltin.ObjectPooling.Editor
{
    [CustomPropertyDrawer(typeof(PoolObject<>))]
    public class PoolObject : GenericObjectDrawer
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