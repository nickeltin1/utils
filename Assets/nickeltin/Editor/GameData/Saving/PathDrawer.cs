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
            SerializedProperty type = property.FindPropertyRelative("_type");
            EditorGUI.PropertyField(position, type, new GUIContent("Save Path"));
        }
    }
}