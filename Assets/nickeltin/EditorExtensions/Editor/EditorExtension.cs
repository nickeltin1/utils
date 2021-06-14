using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Extensions.Editor;
using UnityEditor;
using UnityEngine;

namespace nickeltin.EditorExtensions.Editor
{
    public static class EditorExtension
    {
        public static float line => EditorGUIUtility.singleLineHeight;
        
        public static void DrawScriptField(UnityEditor.Editor editor)
        {
            using (new EditorGUI.DisabledScope(true)) 
            {
                if (editor.target is ScriptableObject so)
                {
                    EditorGUILayout.ObjectField("Script", 
                        MonoScript.FromScriptableObject(so), editor.GetType(), false); 
                }
                
                if (editor.target is MonoBehaviour mb)
                {
                    EditorGUILayout.ObjectField("Script", 
                        MonoScript.FromMonoBehaviour(mb), editor.GetType(), false);
                }
            }
        }
        
        public static void DrawChildProperties(SerializedProperty property, Rect position, 
            List<string> excluding = null)
        {
            DrawChildProperties(property.GetVisibleChildrens(), position, excluding);
        }
        
        public static void DrawChildProperties(SerializedObject @object, Rect position, 
            List<string> excluding = null)
        {
            DrawChildProperties(@object.GetVisibleChildrens(), position, excluding);  
        }

        public static void DrawChildProperties(IEnumerable<SerializedProperty> properties, Rect position, 
            List<string> excluding)
        {
            foreach (var child in properties)
            {
                
                if(excluding != null && excluding.Contains(child.name)) continue;
                
                float propertyHeight =  EditorGUI.GetPropertyHeight(child, false) + 
                                        EditorGUIUtility.standardVerticalSpacing;
                position.y += propertyHeight;
                EditorGUI.PropertyField(position, child, true);
            }
        }

        public static float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property.GetVisibleChildrens(), label);
        }
        
        public static float GetPropertyHeight(SerializedObject @object, GUIContent label)
        {
            return GetPropertyHeight(@object.GetVisibleChildrens(), label);
        }

        public static float GetPropertyHeight(IEnumerable<SerializedProperty> properties, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            foreach (var child in properties)
            {
                totalHeight += EditorGUI.GetPropertyHeight(child, label, true); 
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            return totalHeight;
        }

        public static void DrawBox(Rect original, float addY, float addHeight)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                original.height += addHeight;
                original.y += addY;
                        
                GUI.Box(original, GUIContent.none);
            }
        }
    }
}