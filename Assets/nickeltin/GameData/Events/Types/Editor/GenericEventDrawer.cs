using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Events.Types.Editor
{
    [CustomPropertyDrawer(typeof(Event<>), true)]
    public class GenericEventDrawer : EventDrawer
    {
        private SerializedProperty _invokeData; 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(!VerifyContext(position, property, label)) return;
            
            _invokeData = property.FindPropertyRelative("invokeData");
            
            GUIContent dataLabel = new GUIContent("InvokeData");
            Rect topLine = new Rect(position);
            topLine.height = EditorGUIUtility.singleLineHeight;
            
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            property.isExpanded = EditorGUI.Foldout(topLine, property.isExpanded, label);
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(GetPropertyRect(position), _invokeData, dataLabel, true);
                EditorGUI.indentLevel--;
            }
            
            
            DrawButton(GetButtonRect(position), property);
            
            EditorGUI.EndProperty();
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }

        private Rect GetPropertyRect(Rect source)
        {
            var rect = new Rect(source);
            rect.y += EditorGUIUtility.singleLineHeight;
            rect.height -= EditorGUIUtility.singleLineHeight;
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            rect.height -= EditorGUIUtility.standardVerticalSpacing;
            return rect;
        }
        
        private Rect GetButtonRect(Rect source)
        {
            var rect = new Rect(source);
            rect.x += EditorGUIUtility.labelWidth;
            rect.width -= EditorGUIUtility.labelWidth;
            rect.height = EditorGUIUtility.singleLineHeight;
            return rect;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_invokeData != null && property.isExpanded)
            {
                return base.GetPropertyHeight(property, label) +  EditorGUI.GetPropertyHeight(_invokeData, label, true);
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}