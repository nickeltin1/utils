using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace nickeltin.Localization.Editor
{
    [CustomEditor(typeof(LocalizationManager), editorForChildClasses: true)]
    public class LocalizationManagerEditor : UnityEditor.Editor
    {
        private ReorderableList m_AvailableLanguagesList;
        private SerializedProperty m_availableLanguages;
        private SerializedProperty m_googleAuthenticationFile;
        private SerializedProperty m_currentLanguage;
        private SerializedProperty m_onLanguageChangeEvent;

        private void OnEnable()
        {
            m_availableLanguages = serializedObject.FindProperty("m_availableLanguages");
            m_googleAuthenticationFile = serializedObject.FindProperty("m_googleAuthenticationFile");
            m_currentLanguage = serializedObject.FindProperty("m_currentLanguage");
            m_onLanguageChangeEvent = serializedObject.FindProperty("onLocalizationChangedEvent");
            
            if (m_availableLanguages != null)
            {
                m_AvailableLanguagesList = new ReorderableList
                (
                    serializedObject: serializedObject,
                    elements: m_availableLanguages,
                    draggable: true,
                    displayHeader: true,
                    displayAddButton: true,
                    displayRemoveButton: true
                );
                m_AvailableLanguagesList.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, m_availableLanguages.displayName);
                };
                m_AvailableLanguagesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = m_AvailableLanguagesList.serializedProperty.GetArrayElementAtIndex(index);
                    var position = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(position, element, GUIContent.none);
                };
                m_AvailableLanguagesList.onCanRemoveCallback = (ReorderableList list) =>
                {
                    return list.count > 1;
                };
                m_AvailableLanguagesList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Language", "Adds a language."), false, () =>
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                        serializedObject.ApplyModifiedProperties();
                    });
                    menu.AddItem(new GUIContent("Add used languages", "Adds by searching used languages in assets."), false, () =>
                    {
                        AddUsedLocales();
                        serializedObject.ApplyModifiedProperties();
                    });
                    menu.ShowAsContext();
                };
            }
        }

        public override void OnInspectorGUI()
        {
            if (m_AvailableLanguagesList != null)
            {
                serializedObject.Update();
                
                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_currentLanguage);
                GUI.enabled = true;
                
                EditorGUILayout.Space(20);
                
                m_AvailableLanguagesList.DoLayoutList();
                
                EditorGUILayout.LabelField("Google Translate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_googleAuthenticationFile);
                if (!m_googleAuthenticationFile.objectReferenceValue)
                {
                    EditorGUILayout.HelpBox("If you want to use Google Translate in editor or in-game, attach the service account or API key file claimed from Google Cloud.", MessageType.Info);
                }
                    
                EditorGUILayout.Space(20);
                
                EditorGUILayout.PropertyField(m_onLanguageChangeEvent);
                
                serializedObject.ApplyModifiedProperties();
            }
            else base.OnInspectorGUI();
        }

        private void AddUsedLocales()
        {
            var enumNames = Enum.GetNames(typeof(SystemLanguage));
            var languages = FindUsedLanguages(enumNames);
            m_availableLanguages.arraySize = languages.Count;
            var size = m_availableLanguages.arraySize;
            for (int i = 0; i < size; i++)
            {
                var enumValueIndex = Array.IndexOf(enumNames, languages[i].ToString());
                m_availableLanguages.GetArrayElementAtIndex(i).enumValueIndex = enumValueIndex;
            }
        }

        private List<SystemLanguage> FindUsedLanguages(string[] enumNames)
        {
            var languages = new HashSet<SystemLanguage>();
            for (int i = 0; i < m_availableLanguages.arraySize; i++)
            {
                var enumName = enumNames[m_availableLanguages.GetArrayElementAtIndex(i).enumValueIndex];
                languages.Add((SystemLanguage)Enum.Parse(typeof(SystemLanguage), enumName));
            }

            var localizedAssets = LocalizationManager.FindAllLocalizedAssets();
            foreach (var localizedAsset in localizedAssets)
            {
                foreach (var locale in localizedAsset.LocaleItems)
                {
                    languages.Add(locale.Language);
                }
            }
            return new List<SystemLanguage>(languages);
        }
    }
}
