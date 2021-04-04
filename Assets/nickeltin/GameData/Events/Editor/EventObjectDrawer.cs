using System;
using nickeltin.Editor.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Events.Editor
{
    [CustomPropertyDrawer(typeof(EventObject<>), true)]
    public class EventObjectDrawer : GenericObjectDrawer
    {
        private Type m_eventType;
        private Type[] m_eventParameters;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(EventObject<>), property, ref m_eventType, ref m_eventParameters);
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(position, property, label, m_eventType, m_eventParameters);
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}