﻿using nickeltin.Runtime.GameData.Saving;
using UnityEditor;

namespace nickeltin.Editor.GameData.Saving
{
    [CustomEditor(typeof(SaveRegistry))]
    public class SaveRegistryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorExtension.DrawScriptField(this);
            
            serializedObject.Update();
            
            SaveRegistry obj = (SaveRegistry) target;
            
            EditorGUILayout.HelpBox(
                $"This is {typeof(SaveRegistry).Name} file, items in it will be added to Saves Database, " +
                "but will not be SAVED/LOADED. For all items will use Guid as SaveID", 
                MessageType.Info);
            
            SerializedProperty enteries = serializedObject.FindProperty(SaveRegistry.entries_prop_name);
            EditorGUILayout.PropertyField(enteries);
            
            if (obj.Entries != null && obj.Entries.Count > 0)
            {
                foreach (var save in obj.Entries)
                {
                    if(save != null) save.SaveID.useGUID = true;
                }
            }
            
            bool containsNested = SaveableBase.ContainsNestedSavables(obj.Entries, obj, out var containsItself);
            
            if (containsItself)
            {
                EditorGUILayout.HelpBox(
                    "Your list contains itself, this will cause stack overflow upon LOAD/SAVE", 
                    MessageType.Error);
            }
            else if (containsNested)
            {
                EditorGUILayout.HelpBox(
                    "Your list contains a nested list saves, it can cause unexpected behaviours", 
                    MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}