using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving.Editor
{
    [CustomEditor(typeof(SaveableBase), true)]
    public class SaveableBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            OnInspectorGUI_Internal(this);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }

        public static void OnInspectorGUI_Internal(UnityEditor.Editor target)
        {
            SerializedProperty useGuid = target.serializedObject.FindProperty("m_useGuid");
            SerializedProperty saveID = target.serializedObject.FindProperty("m_saveId");
            SerializedProperty guid = target.serializedObject.FindProperty("m_guid");

            EditorGUILayout.PropertyField(useGuid);
            
            if (useGuid.boolValue)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(guid);
                GUI.enabled = true;
            }
            else EditorGUILayout.PropertyField(saveID);
        }
    }
}