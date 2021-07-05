using nickeltin.Extensions.Types;
using UnityEditor;
using UnityEngine;


namespace nickeltin.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            var enabled = property.FindPropertyRelative("_enabled");

            position.width -= 24;
            EditorGUI.BeginDisabledGroup(!enabled.boolValue);
            EditorGUI.PropertyField(position, value, label, true);
            EditorGUI.EndDisabledGroup();

            position.x += position.width + 24;
            position.width = position.height = EditorGUI.GetPropertyHeight(enabled);
            position.x -= position.width;
            EditorGUI.PropertyField(position, enabled, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var value = property.FindPropertyRelative("_value");
            return EditorGUI.GetPropertyHeight(value);
        }
    }

}