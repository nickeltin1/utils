using nickeltin.EditorExtensions.Editor;
using nickeltin.GameData.DataObjects;
using UnityEditor;

namespace nickeltin.GameData.Editor
{
    [CustomEditor(typeof(DataObject<>), true)]
    public class DataObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty description = serializedObject.FindProperty("_developmentDescription");
            SerializedProperty readOnly = serializedObject.FindProperty("_readonly");
            SerializedProperty value = serializedObject.FindProperty("_value");

            EditorExtension.DrawScriptField(this);

            serializedObject.Update();

            EditorGUILayout.PropertyField(description);
            EditorGUILayout.PropertyField(readOnly);
            
            EditorGUI.BeginDisabledGroup(readOnly.boolValue);
            EditorGUILayout.PropertyField(value, true);
            EditorGUI.EndDisabledGroup();
            
            DrawPropertiesExcluding(serializedObject, "m_Script");
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}