using System;
using nickeltin.Editor.PropertyDrawers;
using nickeltin.Experimental.GlobalVariables.Types;
using UnityEditor;
using UnityEngine;
using Event = nickeltin.Experimental.GlobalVariables.Types.Event;

namespace nickeltin.Experimental.GlobalVariables.Editor
{
    [CustomPropertyDrawer(typeof(GlobalVariable<>))]
    public class GlobalVariableDrawer : GenericObjectDrawer
    {
        private const string m_configIsEmptyMessage = "Source is empty";
        private const string m_configNotAssignedMessage = "Source is not assigned";
        
        private bool m_hasConfigReference = false;

        private Type m_sourceType;
        private Type m_sourceParameter;
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGenericType(typeof(GlobalVariablesRegistry<>), property, ref m_sourceType, ref m_sourceParameter);
            SerializedProperty registry = property.FindPropertyRelative("m_source");
            SerializedProperty entryIndex = property.FindPropertyRelative("m_entryIndex");

            if (Application.isPlaying) GUI.enabled = false;
            
            EditorGUI.BeginChangeCheck();

            //Debug.Log(typeof(GlobalVariable<Event<int>>));
            
            DrawGenericObjectField(GetRegistryRect(position), registry, label, m_sourceType, m_sourceParameter);
            
            m_hasConfigReference = registry.objectReferenceValue != null;
            
            if (m_hasConfigReference)
            {
                VariablesRegistryBase configReference = ((VariablesRegistryBase) registry.objectReferenceValue);
                if (configReference.Count > 0)
                {
                    string[] configEntries = configReference.Keys;
                    entryIndex.intValue = EditorGUI.Popup(GetDropdownRect(position), entryIndex.intValue, configEntries);
                }
                else
                {
                    EditorGUI.HelpBox(GetDropdownRect(position), m_configIsEmptyMessage, MessageType.Warning);   
                }
            }
            else
            {
                EditorGUI.HelpBox(GetDropdownRect(position), m_configNotAssignedMessage, MessageType.Error);   
            }
            
            
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
            
            GUI.enabled = true;
        }

        public Rect GetRegistryRect(Rect source)
        {
            return new Rect(source.x, source.y, (source.width/2f) - EditorGUIUtility.standardVerticalSpacing, source.height);
        }

        public Rect GetDropdownRect(Rect source)
        {
            return new Rect(source.x + source.width/2f, source.y, source.width/2f, source.height);
        }
    }
}