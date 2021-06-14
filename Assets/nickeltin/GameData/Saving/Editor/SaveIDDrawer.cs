using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving.Editor
{
    [CustomPropertyDrawer(typeof(SaveID))]
    public class SaveIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty useGuid = property.FindPropertyRelative("useGUID");
            SerializedProperty saveID = property.FindPropertyRelative("_saveID");
            SerializedProperty guid = property.FindPropertyRelative("_GUID");

            position.height = EditorGUIUtility.singleLineHeight;
            
            EditorGUI.PropertyField(position, useGuid);
            
            position.y += EditorGUIUtility.singleLineHeight;
            
            if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
            {
                Rect contextMenuRect = new Rect(position) {width = EditorGUIUtility.labelWidth};
                ;
                if (contextMenuRect.Contains(Event.current.mousePosition))
                {
                    OnContextMenu(new Rect(Event.current.mousePosition, Vector2.zero), property);
                    Event.current.Use();
                }
            }
            
            if (useGuid.boolValue)
            {
                bool wasGUIEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUI.PropertyField(position, guid);
                GUI.enabled = wasGUIEnabled;
            }
            else EditorGUI.PropertyField(position, saveID);
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        private void OnContextMenu(Rect postion, SerializedProperty property)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Generate GUID"), false, () =>
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Generating GUID");
                ((SaveID)fieldInfo.GetValue(property.serializedObject.targetObject)).GenerateGUID();
            });
            menu.AddItem(new GUIContent("Generate SaveID"), false, () =>
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Generating SaveID");
                ((SaveID)fieldInfo.GetValue(property.serializedObject.targetObject))
                    .GenerateSaveID(property.serializedObject.targetObject.name);
            });
            menu.DropDown(postion);
        }
    }
}