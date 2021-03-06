﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.Editor.Extensions
{
    public static class SerializedPropertyExt
    {
        public static readonly BindingFlags PublicOrNotInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static bool HasAnyElementSameValue(this SerializedProperty array, SerializedProperty key1, int skipIndex)
        {
            int length = array.arraySize;
            for (int i = 0; i < length; i++)
            {
                if (i == skipIndex)
                {
                    continue;
                }

                SerializedProperty key2 = array.GetArrayElementAtIndex(i);
                object key1Value = key1 != null ? key1.GetValue() : null;
                object key2Value = key2 != null ? key2.GetValue() : null;
                if (key1Value == null ? key2Value == null : key1Value.Equals(key2Value))
                {
                    return true;
                }
            }
            return false;
        }

        public static object GetValue(this object source, string name)
        {
            if (source == null)
            {
                return null;
            }

            Type type = source.GetType();
            while (type != null)
            {
                FieldInfo f = type.GetField(name, PublicOrNotInstance);
                if (f != null)
                {
                    return f.GetValue(source);
                }

                PropertyInfo p = type.GetProperty(name, PublicOrNotInstance | BindingFlags.IgnoreCase);
                if (p != null)
                {
                    return p.GetValue(source, null);
                }
                type = type.BaseType;
            }
            return null;
        }

        public static object GetValue(this object source, string name, int index)
        {
            IEnumerable enumerable = GetValue(source, name) as IEnumerable;
            if (enumerable == null)
            {
                return null;
            }

            IEnumerator enm = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext())
                {
                    return null;
                }
            }
            return enm.Current;
        }
        
        public static object GetTargetObject(this SerializedProperty prop)
        {
            object targetObj = prop.serializedObject.targetObject;
            string[] elements = prop.propertyPath.Replace(".Array.data[", "[").Split('.');
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Contains("["))
                {
                    string elementName = elements[i].Substring(0, elements[i].IndexOf("["));
                    int index = Convert.ToInt32(elements[i].Substring(elements[i].IndexOf("[")).Replace(
                        "[", "").Replace("]", ""));
                    targetObj = GetValue(targetObj, elementName, index);
                }
                else
                {
                    targetObj = GetValue(targetObj, elements[i]);
                }
            }
            return targetObj;
        }

        public static object GetValue(this SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return prop.intValue;
                case SerializedPropertyType.Float:
                    return prop.floatValue;
                case SerializedPropertyType.String:
                    return prop.stringValue;
                case SerializedPropertyType.Enum:
                    return prop.enumValueIndex;
                case SerializedPropertyType.Boolean:
                    return prop.boolValue;
                case SerializedPropertyType.Color:
                    return prop.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return prop.objectReferenceValue;
                case SerializedPropertyType.Vector2:
                    return prop.vector2Value;
                case SerializedPropertyType.Vector3:
                    return prop.vector3Value;
                case SerializedPropertyType.Vector4:
                    return prop.vector4Value;
                case SerializedPropertyType.Quaternion:
                    return prop.quaternionValue;
                case SerializedPropertyType.Vector2Int:
                    return prop.vector2IntValue;
                case SerializedPropertyType.Vector3Int:
                    return prop.vector3IntValue;
                case SerializedPropertyType.ExposedReference:
                    return prop.exposedReferenceValue;
                case SerializedPropertyType.ArraySize:
                    return prop.arraySize;
                case SerializedPropertyType.Rect:
                    return prop.rectValue;
                case SerializedPropertyType.RectInt:
                    return prop.rectIntValue;
                case SerializedPropertyType.Bounds:
                    return prop.boundsValue;
                case SerializedPropertyType.BoundsInt:
                    return prop.boundsIntValue;
                case SerializedPropertyType.FixedBufferSize:
                    return prop.fixedBufferSize;
                case SerializedPropertyType.AnimationCurve:
                    return prop.animationCurveValue;
            }

            string typ = prop.type;
            if (typ == "double")
            {
                return prop.doubleValue;
            }
            if (typ == "long")
            {
                return prop.longValue;
            }
            return prop.GetTargetObject();
        }
        
        public static Type[] GetGenericParametersTypes(this SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            string[] path = property.GetFullPropertyPath();
            string firstName = path[0];
            FormatArrayNaming(ref firstName);
            
            FieldInfo fieldInfo = null;
            fieldInfo = GetFieldInfo(firstName, parentType);
            
            while (fieldInfo == null && parentType.BaseType != null)
            {
                parentType = parentType.BaseType;
                fieldInfo = GetFieldInfo(firstName, parentType);
            }

            Type type = fieldInfo.FieldType;
            
            if (type.IsArray) type = type.GetElementType();

            for (int i = 1; i < path.Length; i++)
            {
                if (type != null) parentType = type;
                type = GetFieldInfo(path[i], parentType).FieldType;
            }

            if (type != null)
            {
                return TypeExt.GetGenericHierarchy(type.ToString());
            }
            
            return null;
        }

        public static Type GetObjectType(this SerializedProperty property) => property.GetObjectType(0);

        public static Type GetParentType(this SerializedProperty property) => property.GetObjectType(1);
        
        /// <param name="depth">use 0 if you want to get object itself object</param>
        /// <returns></returns>
        public static Type GetObjectType(this SerializedProperty property, int depth)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            string[] path = property.GetFullPropertyPath();
            Type type = null;
            for (int i = 0; i < path.Length - depth; i++)
            {
                string name = path[i];
                FormatArrayNaming(ref name);
                FieldInfo fieldInfo = null;
                fieldInfo = GetFieldInfo(name, parentType);
                while (fieldInfo == null && parentType.BaseType != null)
                {
                    parentType = parentType.BaseType;
                    fieldInfo = GetFieldInfo(name, parentType);
                }
                type = fieldInfo.FieldType;
                if (type.IsArray) type = type.GetElementType();
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    type = type.GetGenericArguments().First();
                }
                parentType = type;
               
            }
            return type != null ? type : typeof(Object);
        }
        
        public static string[] GetFullPropertyPath(this SerializedProperty property)
        {
            return property.propertyPath.Replace(".Array.data[", "[").Split('.');
        }

        private static FieldInfo GetFieldInfo(string fieldName, Type from)
        {
            return from.GetField(fieldName, PublicOrNotInstance);
        }

        private static void FormatArrayNaming(ref string name)
        {
            if (name.Contains("[")) name = name.Substring(0, name.IndexOf("["));
        }
        
        /// <summary>
        /// Gets all children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetChildrens(this SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();
            SerializedProperty nextSiblingProperty = serializedProperty.Copy();
            {
                nextSiblingProperty.Next(false);
            }
 
            if (currentProperty.Next(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;
 
                    yield return currentProperty;
                }
                while (currentProperty.Next(false));
            }
        }
 
        /// <summary>
        /// Gets visible children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildrens(this SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();
            SerializedProperty nextSiblingProperty = serializedProperty.Copy();
            {
                nextSiblingProperty.NextVisible(false);
            }
 
            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty)) break;
 
                    yield return currentProperty;
                }
                while (currentProperty.NextVisible(false));
            }
        }
        
        public static bool Contains(this SerializedProperty serializedProperty, Object item, out int atIndex)
        {
            if(!serializedProperty.isArray) 
                throw new Exception(serializedProperty.propertyPath + " not an array");
            
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                if (serializedProperty.objectReferenceValue == item)
                {
                    atIndex = i;
                    return true;
                }
            }

            atIndex = -1;
            return false;
        }
    }
}