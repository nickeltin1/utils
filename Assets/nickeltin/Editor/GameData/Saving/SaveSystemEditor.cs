using System.IO;
using nickeltin.Runtime.GameData.Saving;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace nickeltin.Editor.GameData.Saving
{
    [CustomEditor(typeof(SaveSystem))]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private static readonly string ENABLED_NAME = SaveSystem.enabled_prop_name;
        private static readonly string SUBFOLDERS_NAME = SaveSystem.sub_folders_prop_name;
        private static readonly string DIRECTORY_SETTINGS_NAME = SaveSystem.directory_settings_prop_name;
        
        private static readonly string[] _excluding = {"m_Script", ENABLED_NAME, SUBFOLDERS_NAME, DIRECTORY_SETTINGS_NAME};
        
        private SaveSystem _saveSystem;
        private ReorderableList _reorderableList;
        private SerializedProperty _subfolders;
        
        private void OnEnable()
        {
            _saveSystem = (SaveSystem) target;
            _subfolders = serializedObject.FindProperty(SUBFOLDERS_NAME);
            _reorderableList = new ReorderableList(serializedObject, _subfolders, 
                false, true, true, true)
            {
                drawHeaderCallback = DrawHeaderCallback,
                drawElementCallback = DrawElementCallback,
                onCanRemoveCallback = list => _subfolders.arraySize >= 2
            };
        }

        #region SubFolders Callbacks
        
        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            rect.height -= (EditorGUIUtility.standardVerticalSpacing * 2);
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            SerializedProperty subFolder = _subfolders.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, subFolder.FindPropertyRelative(SaveSystem.SubFolder.name_prop_name), 
                new GUIContent("Sub Folder " + index));
        }
        
        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, new GUIContent("Sub Folders"));
        }
        
        #endregion
        
       
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorExtension.DrawScriptField(this);
            
            SerializedProperty enabled = serializedObject.FindProperty(ENABLED_NAME);
            EditorGUILayout.PropertyField(enabled);

            EditorGUI.BeginDisabledGroup(!enabled.boolValue);
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(DIRECTORY_SETTINGS_NAME), true);
                
                _reorderableList.DoLayoutList();
                
                DrawPropertiesExcluding(serializedObject, _excluding);

                DrawButtons();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        
        

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Refresh saves list"))
                {
                    Undo.RecordObject(target, "Refreshing saves");
                    _saveSystem.RefreshEventsList();
                }

                if (GUILayout.Button("Open Saves Folder"))
                {
                    Directory.CreateDirectory(_saveSystem.CurrentSavePath);
                    EditorUtility.RevealInFinder(_saveSystem.CurrentSavePath);
                }

                if (GUILayout.Button("Delete all saves"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete all saves?", 
                        "Yes", "No"))
                    {
                        _saveSystem.DeleteAllSaves();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
                
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(!Application.isPlaying);
                {
                    if (GUILayout.Button("Save"))
                    {
                        _saveSystem.SaveAll();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}