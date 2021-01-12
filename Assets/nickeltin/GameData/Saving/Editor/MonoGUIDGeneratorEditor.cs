using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving.Editor
{
    [CustomEditor(typeof(MonoGUIDGenerator))]
    public class MonoGUIDGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("Generates GUID's for itself, and for all childrens", MessageType.Info);
            if (GUILayout.Button("Generate GUID's"))
            {
                (target as MonoGUIDGenerator)?.GenerateGUIDs();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}