using System;
using nickeltin.Extensions;
using nickeltin.Editor.Extensions;
using nickeltin.Runtime.GameData.VariablesRefrences;
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

                if (parameters != null)
                {
                    type = baseType.GetGenericInheritor(parameters);
                    if (type == null) return false;
                }
                else type = property.GetObjectType();
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
                position.width -= EditorGUIUtility.labelWidth + 2;
            }

            string type = "";
            if (parameters != null) for (var i = 0; i < parameters.Length; i++) type += parameters[i];

            EditorGUI.HelpBox(position, $"Generic type with parameter {type} not found", MessageType.Error);
        }

        protected bool IsInsideVariableReference(SerializedProperty property)
        {
            Type parentType = property.GetParentType();
            if (parentType.BaseType == typeof(VariableReferenceBase))
            {
                return true;
            }

            return false;
        }
    }
}