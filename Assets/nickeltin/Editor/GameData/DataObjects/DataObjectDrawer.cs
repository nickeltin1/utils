using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.GameData.DataObjects;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.DataObjects
{
    [CustomPropertyDrawer(typeof(DataObject<>), true)]
    public class DataObjectDrawer : GenericObjectDrawer
    {
        private Type _objectType;
        private Type[] _objectParameters;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent _label)
        {
            CacheGenericType(typeof(DataObject<>), property, ref _objectType, ref _objectParameters);
            
            GUIContent label = new GUIContent(_label);
            
            if (!IsInsideVariableReference(property)) label.text = property.displayName;

            Rect topLine = new Rect(position) {height = EditorGUIUtility.singleLineHeight};

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(topLine, property, label, _objectType, _objectParameters);
            
            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }
    }
}