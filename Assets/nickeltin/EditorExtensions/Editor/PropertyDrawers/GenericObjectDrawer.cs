﻿using System;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.PropertyDrawers
{
    public abstract class GenericObjectDrawer : PropertyDrawer
    {
        protected bool CacheGenericType(Type baseType, SerializedProperty property, ref Type type, 
            ref Type[] parameters)
        {
            if (type == null)
            {
                parameters = property.GetGenericParametersTypes();
                type = baseType.GetGenericInheritor(parameters);
                if (type == null) return false;
            }

            return true;
        }

        protected void DrawGenericObjectField(Rect position, SerializedProperty property, GUIContent label, Type type, 
            Type[] parameters)
        {
            if (type != null) EditorGUI.ObjectField(position, property, type, label);
            else DrawGenericNotFoundError(position, parameters, label);
        }

        protected void DrawGenericNotFoundError(Rect position, Type[] parameters, GUIContent label)
        {
            if (label != GUIContent.none)
            {
                EditorGUI.LabelField(position, label);
                position.x += EditorGUIUtility.labelWidth + 2;
            }

            string type = "";
            foreach (var param in parameters) type += param;

            EditorGUI.HelpBox(position, $"Generic type with parameter {type} not found", MessageType.Error);
        }
    }
}