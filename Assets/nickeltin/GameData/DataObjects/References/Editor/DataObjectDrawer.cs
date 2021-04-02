using System;
using nickeltin.Editor.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.DataObjects.Editor
{
    [CustomPropertyDrawer(typeof(DataObject<>), true)]
    public class DataObjectDrawer : GenericObjectDrawer
    {
        private Type m_objectType;
        private Type m_objectParameter;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(DataObject<>), property, ref m_objectType, ref m_objectParameter);
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(position, property, label, m_objectType, m_objectParameter);
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}