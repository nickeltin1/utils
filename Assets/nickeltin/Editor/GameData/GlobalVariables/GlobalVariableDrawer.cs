using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.GameData.GlobalVariables;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.GlobalVariables
{
    [CustomPropertyDrawer(typeof(GlobalVar<>))]
    public class GlobalVariableDrawer : GenericObjectDrawer
    {
        private const string _registryIsEmptyMessage = "Registry is empty";
        private const string _registryNotAssignedMessage = "Registry is not assigned";

        private Type _sourceType;
        private Type[] _sourceParameters;
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool typeFound = CacheGenericType(typeof(GlobalVariablesRegistry<>), 
                property, ref _sourceType, ref _sourceParameters);
            
            SerializedProperty registry = property.FindPropertyRelative("_registry");
            SerializedProperty entryIndex = property.FindPropertyRelative("_entryIndex");

            Rect topLine = new Rect(position) {height = EditorGUIUtility.singleLineHeight};

            bool GUIWasEnabled = GUI.enabled;
            if (Application.isPlaying) GUI.enabled = false;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(typeFound ? GetRegistryRect(topLine) : topLine, 
                registry, label, _sourceType, _sourceParameters);

            Rect dropdownRect = GetDropdownRect(topLine);
            
            GUI.Box(dropdownRect, GUIContent.none);

            if (registry.objectReferenceValue != null)
            {
                GlobalVariablesRegistryBase registryReference = ((GlobalVariablesRegistryBase) registry.objectReferenceValue);
                if (registryReference.Assigned && registryReference.Count > 0)
                {
                    string[] configEntries = registryReference.Keys;
                    var guiStyle = GUI.skin.button;
                    bool indented = false;
                    if (EditorGUI.indentLevel > 0)
                    {
                        indented = true;
                        EditorGUI.indentLevel--;
                    }
                    
                    
                    entryIndex.intValue = EditorGUI.Popup(dropdownRect, entryIndex.intValue, configEntries, guiStyle);
                    
                    
                    if(indented) EditorGUI.indentLevel++;
                }
                else
                {
                    EditorGUI.HelpBox(dropdownRect, _registryIsEmptyMessage, MessageType.Warning);   
                }
            }
            else if (typeFound)
            {
                EditorGUI.HelpBox(dropdownRect, _registryNotAssignedMessage, MessageType.Error);   
            }
            
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            
            GUI.enabled = GUIWasEnabled;
        }

        private Rect GetRegistryRect(Rect source)
        {
            float width = source.width / 1.5f;
            return new Rect(source.x, source.y, width - EditorGUIUtility.standardVerticalSpacing, source.height);
        }

        private Rect GetDropdownRect(Rect source)
        {
            float width = source.width / 1.5f;
            return new Rect(source.x + width, source.y, width / 2, source.height);
        }
    }
}