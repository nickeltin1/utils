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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent _label)
        {
            CacheGenericType(typeof(DataObject<>), property, ref m_objectType, ref m_objectParameters);
            
            GUIContent label = new GUIContent(_label);
            
            if (!IsInsideVariableReference(property)) label.text = property.displayName;

            Rect topLine = new Rect(position) {height = EditorGUIUtility.singleLineHeight};

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            
            DrawGenericObjectField(topLine, property, label, m_objectType, m_objectParameters);
            
            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}