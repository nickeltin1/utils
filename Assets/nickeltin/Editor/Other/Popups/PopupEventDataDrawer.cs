using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.Other.Popups;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Other.Popups
{
    [CustomPropertyDrawer(typeof(PopupEventData))]
    public class PopupEventDataDrawer : LayoutPropertyDrawer
    {
        protected override void DrawChildProperties(Rect position, SerializedProperty property)
        {
            SerializedProperty overrideTweeningSettings = property.FindPropertyRelative("overrideTweeningSettings");
            SerializedProperty useRandomText = property.FindPropertyRelative("useRandomText");
            SerializedProperty usesWorldPos = property.FindPropertyRelative("usesWorldPos");
            
            EditorGUILayout.PropertyField(overrideTweeningSettings);
            if (overrideTweeningSettings.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("tweenSettings"));
            }

            EditorGUILayout.PropertyField(useRandomText);
            if (useRandomText.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("_randomText"));
            }
            else
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("text"));
            }
            
            EditorGUILayout.PropertyField(usesWorldPos);
            if (usesWorldPos.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("worldPos"));
            }

            EditorGUILayout.PropertyField(property.FindPropertyRelative("sprite"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("color"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_width"));
        }
    }
}