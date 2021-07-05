using nickeltin.Runtime.GameData.Saving;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.Saving
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