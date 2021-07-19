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
            SerializedProperty overrideTweeningSettings = property.FindPropertyRelative(nameof(PopupEventData.overrideTweeningSettings));
            SerializedProperty useRandomText = property.FindPropertyRelative(nameof(PopupEventData.useRandomText));
            SerializedProperty usesWorldPos = property.FindPropertyRelative(nameof(PopupEventData.usesWorldPos));
            
            EditorGUILayout.PropertyField(overrideTweeningSettings);
            if (overrideTweeningSettings.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(PopupEventData.tweenSettings)));
            }

            EditorGUILayout.PropertyField(useRandomText);
            if (useRandomText.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(PopupEventData.rand_text_prop_name));
            }
            else
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(PopupEventData.text)));
            }
            
            EditorGUILayout.PropertyField(usesWorldPos);
            if (usesWorldPos.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(PopupEventData.worldPos)));
            }

            EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(PopupEventData.sprite)));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(PopupEventData.color)));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(PopupEventData.width_prop_name));
        }
    }
}