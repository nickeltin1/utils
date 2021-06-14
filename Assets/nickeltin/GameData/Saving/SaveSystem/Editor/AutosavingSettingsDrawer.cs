using System.Collections.Generic;
using nickeltin.EditorExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving.Editor
{
    [CustomPropertyDrawer(typeof(SaveSystem.AutosavingSettings))]
    public class AutosavingSettingsDrawer : PropertyDrawer
    {
        private static readonly List<string> _excluding = new List<string> {"autosaveEnabled", "autosaveInterval"};
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorExtension.DrawBox(position, EditorGUIUtility.singleLineHeight, -EditorGUIUtility.singleLineHeight);
            
            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(position, property.isExpanded, label);

            if (property.isExpanded)
            {
                DrawChildProperties(position, property);
            }
            
            EditorGUI.EndFoldoutHeaderGroup();
        }

        private void DrawChildProperties(Rect position, SerializedProperty property)
        {
            position.y += EditorGUIUtility.singleLineHeight;

            SerializedProperty autosaveInterval = property.FindPropertyRelative("autosaveInterval");
            SerializedProperty autosaveEnabled = property.FindPropertyRelative("autosaveEnabled");
            
            
            using (new EditorGUI.IndentLevelScope())
            {
                var localRect = new Rect(position);
                
                localRect = EditorGUI.PrefixLabel(localRect, new GUIContent(autosaveInterval.displayName));
                var toggleRect = localRect;
                toggleRect.x -= 15;
                toggleRect.width = 30;
                
                autosaveEnabled.boolValue = EditorGUI.Toggle(toggleRect, autosaveEnabled.boolValue);
                
                localRect.width -= 15;
                localRect.x += 15;

                if (autosaveEnabled.boolValue)
                {
                    EditorGUI.Slider(localRect, autosaveInterval, 
                        SaveSystem.AutosavingSettings.MIN_AUTOSAVE_INTERVAL,
                        SaveSystem.AutosavingSettings.MAX_AUTOSAVE_INTERVAL, GUIContent.none);
                }
                else
                {
                    var previousColor = GUI.color;
                    GUI.color = Color.red;
                    EditorGUI.DropShadowLabel(localRect, "Autosave disabled");
                    GUI.color = previousColor;
                }
                

                
                EditorExtension.DrawChildProperties(property, position, _excluding);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}