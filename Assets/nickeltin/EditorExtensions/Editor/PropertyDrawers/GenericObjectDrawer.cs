using System;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.PropertyDrawers
{
    public abstract class GenericObjectDrawer : PropertyDrawer
    {
        protected bool CacheGenericType(Type baseType, SerializedProperty property, ref Type typeSource, 
            ref Type parameterSource)
        {
            if (typeSource == null)
            {
                parameterSource = property.GetGenericParameterType();
                typeSource = baseType.GetGenericInheritor(parameterSource);
                if (typeSource == null) return false;
            }

            return true;
        }

        protected void DrawGenericObjectField(Rect position, SerializedProperty property, GUIContent label, Type type, 
            Type parameter)
        {
            if (type != null) EditorGUI.ObjectField(position, property, type, label);
            else DrawGenericNotFoundError(position, parameter, label);
        }

        protected void DrawGenericNotFoundError(Rect position, Type type, GUIContent label)
        {

            if (label != GUIContent.none)
            {
                EditorGUI.LabelField(position, label);
                position.x += EditorGUIUtility.labelWidth + 2;
            }
            EditorGUI.HelpBox(position, $"Generic type with parameter {type} not found", MessageType.Error);
        }
    }
}