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
            SerializedProperty description = serializedObject.FindProperty(DataObject<int>.dev_desc_prop_name);
            SerializedProperty readOnly = serializedObject.FindProperty(DataObject<int>.readonly_prop_name);
            SerializedProperty value = serializedObject.FindProperty(DataObject<int>.value_prop_name);

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