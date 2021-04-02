using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.DataObjects.Editor
{
    [CustomPropertyDrawer(typeof(VariableReferenceBase), true)]
    public sealed class VariableReferenceDrawer : PropertyDrawer
    {
        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            // Type variableType = property.GetGenericParameterType();
            //
            // label.text += $" ({variableType})";
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty referenceType = property.FindPropertyRelative("m_referenceType");
            SerializedProperty constantValue = property.FindPropertyRelative("m_constantValue");
            SerializedProperty dataObject = property.FindPropertyRelative("m_dataObject");
            SerializedProperty globalVariable = property.FindPropertyRelative("m_globalVariable");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, referenceType.enumValueIndex, referenceType.enumDisplayNames, popupStyle);

            referenceType.enumValueIndex = result; 
            
            if (referenceType.enumValueIndex == 0)
            {
				EditorGUI.PropertyField(position, constantValue, GUIContent.none);
            }
            else if (referenceType.enumValueIndex == 1)
            {
				EditorGUI.PropertyField(position, dataObject, GUIContent.none);
            }
            else if (referenceType.enumValueIndex == 2)
            {
                EditorGUI.PropertyField(position, globalVariable, GUIContent.none);
            }
            

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}