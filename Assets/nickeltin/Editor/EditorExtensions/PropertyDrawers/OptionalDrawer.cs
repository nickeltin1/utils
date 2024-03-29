﻿using nickeltin.Extensions.Types;
using UnityEditor;
using UnityEngine;


namespace nickeltin.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalDrawer : PropertyDrawer
    {
        private const int TOGGLE_WIDTH = 24;
        private const int INDENT_STEP = 15;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var value = property.FindPropertyRelative(Optional<int>.value_prop_name);
            var enabled = property.FindPropertyRelative(Optional<int>.enabled_prop_name);

            position.width -= TOGGLE_WIDTH;
            EditorGUI.BeginDisabledGroup(!enabled.boolValue);
            EditorGUI.PropertyField(position, value, label, true);
            EditorGUI.EndDisabledGroup();

            position.x += position.width + TOGGLE_WIDTH;
            position.width = position.height = EditorGUI.GetPropertyHeight(enabled);
            position.x -= position.width + (EditorGUI.indentLevel * INDENT_STEP);
            EditorGUI.PropertyField(position, enabled, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var value = property.FindPropertyRelative(Optional<int>.value_prop_name);
            return EditorGUI.GetPropertyHeight(value);
        }
    }

}