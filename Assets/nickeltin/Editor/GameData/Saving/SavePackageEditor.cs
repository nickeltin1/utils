﻿using nickeltin.Runtime.GameData.Saving;
using UnityEditor;

namespace nickeltin.Editor.GameData.Saving
{
    [CustomEditor(typeof(SavePackage))]
    public class SavePackageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
         
            base.OnInspectorGUI();

            SavePackage obj = (SavePackage) target;
            
            EditorGUILayout.HelpBox(
                $"This is {typeof(SavePackage).Name} file, items in it will be compiled into one save file." +
                $"Each item stil be added to database as separate object", 
                MessageType.Info);
            
            bool containsNested = SaveableBase.ContainsNestedSavables(obj.Saves, obj, out var containsItself);
            
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


            SerializedProperty saves = serializedObject.FindProperty(SavePackage.saves_prop_name);

            EditorGUILayout.PropertyField(saves);
                
            serializedObject.ApplyModifiedProperties();
        }
    }
}