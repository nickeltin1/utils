using nickeltin.Runtime.GameData.DataObjects;
using UnityEditor;
using UnityEngine;

namespace  nickeltin.Editor.GameData.DataObjects
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

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            if (GUILayout.Button("Invoke Update"))
            {
                ((DataObjectBase)serializedObject.targetObject).InvokeUpdate();
            }
            EditorGUI.EndDisabledGroup();
            
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