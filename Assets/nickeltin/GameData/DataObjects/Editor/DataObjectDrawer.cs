using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.GameData.DataObjects;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor
{
    [CustomPropertyDrawer(typeof(DataObject<>), true)]
    public class DataObjectDrawer : GenericObjectDrawer
    {
        private Type m_objectType;
        private Type[] m_objectParameters;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(DataObject<>), property, ref m_objectType, ref m_objectParameters);
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(position, property, label, m_objectType, m_objectParameters);
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}