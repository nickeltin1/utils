using System.Collections.Generic;
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
        private static readonly string[] _excluding = {
            "m_Script", 
            SaveSystem.enabled_prop_name, 
            SaveSystem.sub_folders_prop_name, 
            SaveSystem.directory_settings_prop_name,
            SaveSystem.log_events_prop_name
        };
        
        private const string SUB_FOLDER_EXPAND_KEY = nameof(SUB_FOLDER_EXPAND_KEY);
        
        private SaveSystem _saveSystem;
        private ReorderableList _reorderableList;
        private SerializedProperty _subFolders;
        
        private void OnEnable()
        {
            _saveSystem = (SaveSystem) target;
            _subFolders = serializedObject.FindProperty(SaveSystem.sub_folders_prop_name);
            _reorderableList = new ReorderableList(serializedObject, _subFolders, 
                false, false, true, true)
            {
                drawElementCallback = DrawElementCallback,
                onCanRemoveCallback = list => _subFolders.arraySize >= 2,
                onAddCallback = OnAddCallback
            };
        }

        
        #region SubFolders Callbacks
        
        private void OnAddCallback(ReorderableList list)
        {
            _subFolders.arraySize++;
            SerializedProperty subFolder = _subFolders.GetArrayElementAtIndex(_subFolders.arraySize - 1);
            string uniqueName = SaveSystem.Core.GenerateUniqueName();
            subFolder.FindPropertyRelative(SaveSystem.SubFolder.name_prop_name).stringValue = uniqueName;
            subFolder.FindPropertyRelative(SaveSystem.SubFolder.old_name_prop_name).stringValue = uniqueName;

            serializedObject.ApplyModifiedProperties();
            
            SaveSystem.Core.CreateFolder(_saveSystem.GetSubFolderPath(_subFolders.arraySize - 1));
        }
        
        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            rect.height -= (EditorGUIUtility.standardVerticalSpacing * 2);
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            SerializedProperty subFolder = _subFolders.GetArrayElementAtIndex(index);

            SerializedProperty nameProp = subFolder.FindPropertyRelative(SaveSystem.SubFolder.name_prop_name);
            SerializedProperty oldNameProp = subFolder.FindPropertyRelative(SaveSystem.SubFolder.old_name_prop_name);
            
            
            EditorGUI.PropertyField(rect, nameProp, new GUIContent("Sub Folder " + index));

            if (nameProp.stringValue != oldNameProp.stringValue)
            {
                if (string.IsNullOrWhiteSpace(nameProp.stringValue) || CheckForDuplicateSubFolders())
                {
                    nameProp.stringValue = oldNameProp.stringValue;
                    return;
                }
                
                serializedObject.ApplyModifiedProperties();
                
                Debug.Log("Rename");
                Debug.Log(nameProp.stringValue);
                string oldPath = _saveSystem.RootSavePath + oldNameProp.stringValue;
                string newPath = _saveSystem.GetSubFolderPath(index);
                if (Directory.Exists(oldPath) && newPath != oldPath && !Directory.Exists(newPath))
                {
                    Directory.Move(oldPath, newPath);
                }
                else if(!Directory.Exists(newPath))
                {
                    SaveSystem.Core.CreateFolder(newPath);
                }
                
                oldNameProp.stringValue = nameProp.stringValue;
            }
            
        }

        #endregion


        private bool CheckForDuplicateSubFolders()
        {
            HashSet<string> names = new HashSet<string>();
            for (int i = 0; i < _subFolders.arraySize; i++)
            {
                string n = _subFolders.GetArrayElementAtIndex(i)
                    .FindPropertyRelative(SaveSystem.SubFolder.name_prop_name).stringValue;
                if (!names.Add(n)) return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorExtension.DrawScriptField(this);

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            
            
            SerializedProperty enabled = serializedObject.FindProperty(SaveSystem.enabled_prop_name);
            EditorGUILayout.PropertyField(enabled);

            EditorGUI.BeginDisabledGroup(!enabled.boolValue);
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(SaveSystem.log_events_prop_name));
                
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty(SaveSystem.directory_settings_prop_name), true);


                bool expanded = EditorGUILayout.BeginFoldoutHeaderGroup(
                    EditorPrefs.GetBool(SUB_FOLDER_EXPAND_KEY, false),
                    new GUIContent("Sub Folders"));
                
                EditorPrefs.SetBool(SUB_FOLDER_EXPAND_KEY, expanded);
                if(expanded)
                {
                    _reorderableList.DoLayoutList();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();


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
                    SerializedProperty saves = serializedObject.FindProperty(SaveSystem.saves_prop_name);
                    saves.ClearArray();
                    var foundChilds = Resources.FindObjectsOfTypeAll<SaveableBase>();
                    saves.arraySize = foundChilds.Length;
                    for (var i = 0; i < foundChilds.Length; i++)
                    {
                        saves.GetArrayElementAtIndex(i).objectReferenceValue = foundChilds[i];
                    }
                }

                if (GUILayout.Button("Open Saves Folder"))
                {
                    SaveSystem.Core.CreateFolder(_saveSystem.CurrentSavePath);
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