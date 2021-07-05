using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.GameData.Events;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.Events
{
    [CustomPropertyDrawer(typeof(EventObject<>), true)]
    public class EventObjectDrawer : GenericObjectDrawer
    {
        private Type _eventType;
        private Type[] _eventParameters;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(EventObject<>), property, ref _eventType, ref _eventParameters);
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(position, property, label, _eventType, _eventParameters);
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}