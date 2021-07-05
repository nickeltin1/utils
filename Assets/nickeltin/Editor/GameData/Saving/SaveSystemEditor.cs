using System.IO;
using nickeltin.Editor;
using nickeltin.Runtime.GameData.Saving;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.Saving
{
    [CustomEditor(typeof(SaveSystem))]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private static readonly string[] _excluding = {"m_Script"};  
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SaveSystem saveSystem = (SaveSystem) target;
            
            EditorExtension.DrawScriptField(this);
            
            SerializedProperty enabled = serializedObject.FindProperty("_enabled");
            EditorGUILayout.PropertyField(enabled);

            GUI.enabled = enabled.boolValue;

            DrawPropertiesExcluding(serializedObject, _excluding);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Refresh saves list"))
            {
                Undo.RecordObject(target, "Refreshing saves");
                SaveSystem.RefreshEventsList(saveSystem);
            }

            if (GUILayout.Button("Open Saves Folder"))
            {
                Directory.CreateDirectory(saveSystem.SavePath);
                EditorUtility.RevealInFinder(saveSystem.SavePath);
            }

            if (GUILayout.Button("Delete all saves"))
            {
                if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete all saves?", 
                    "Yes", "No"))
                {
                    saveSystem.DeleteSaves();
                }
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
    }
}