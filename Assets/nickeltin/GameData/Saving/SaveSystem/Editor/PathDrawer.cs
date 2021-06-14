using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving.Editor
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