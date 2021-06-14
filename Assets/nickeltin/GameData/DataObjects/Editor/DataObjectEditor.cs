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
            SerializedProperty description = serializedObject.FindProperty("m_developmentDescription");
            SerializedProperty readOnly = serializedObject.FindProperty("m_readonly");
            SerializedProperty value = serializedObject.FindProperty("m_value");

            EditorExtension.DrawScriptField(this);

            serializedObject.Update();

            EditorGUILayout.PropertyField(description);
            EditorGUILayout.PropertyField(readOnly);
            
            EditorGUI.BeginDisabledGroup(readOnly.boolValue);
            EditorGUILayout.PropertyField(value, true);
            EditorGUI.EndDisabledGroup();
            
            DrawPropertiesExcluding(serializedObject, "m_Script", "m_value", "m_readonly");
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}