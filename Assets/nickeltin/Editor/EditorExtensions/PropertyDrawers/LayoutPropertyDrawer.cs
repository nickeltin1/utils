using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.PropertyDrawers
{
    public abstract class LayoutPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUILayout.BeginVertical(GUI.skin.window);
            DrawChildProperties(position, property);
            EditorGUILayout.EndVertical();
            
            EditorGUI.LabelField(position, label, EditorStyles.centeredGreyMiniLabel);
        }

         protected abstract void DrawChildProperties(Rect position, SerializedProperty property);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0;
    }
}