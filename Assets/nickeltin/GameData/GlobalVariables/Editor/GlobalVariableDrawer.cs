using System;
using nickeltin.Editor.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.Editor
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
            
            if (Application.isPlaying) GUI.enabled = false;
            
            EditorGUI.BeginChangeCheck();
            
            DrawGenericObjectField(typeFound ? GetRegistryRect(topLine) : topLine, 
                registry, label, _sourceType, _sourceParameters);

            if (registry.objectReferenceValue != null)
            {
                GlobalVariablesRegistryBase registryReference = ((GlobalVariablesRegistryBase) registry.objectReferenceValue);
                if (registryReference.Count > 0)
                {
                    string[] configEntries = registryReference.Keys;
                    entryIndex.intValue = EditorGUI.Popup(GetDropdownRect(topLine), entryIndex.intValue, configEntries);
                }
                else
                {
                    EditorGUI.HelpBox(GetDropdownRect(topLine), _registryIsEmptyMessage, MessageType.Warning);   
                }
            }
            else if (typeFound)
            {
                EditorGUI.HelpBox(GetDropdownRect(topLine), _registryNotAssignedMessage, MessageType.Error);   
            }


            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

            GUI.enabled = true;
        }

        public Rect GetRegistryRect(Rect source)
        {
            float width = source.width / 1.5f;
            return new Rect(source.x, source.y, width - EditorGUIUtility.standardVerticalSpacing, source.height);
        }

        public Rect GetDropdownRect(Rect source)
        {
            float width = source.width / 1.5f;
            return new Rect(source.x + width, source.y, width / 2, source.height);
        }
    }
}