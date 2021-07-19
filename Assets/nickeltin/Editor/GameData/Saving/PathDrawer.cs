using nickeltin.Runtime.GameData.Saving;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.Saving
{
    [CustomPropertyDrawer(typeof(SaveSystem.Path))]
    public class PathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty type = property.FindPropertyRelative(SaveSystem.Path.type_prop_name);
            EditorGUI.PropertyField(position, type, label);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}