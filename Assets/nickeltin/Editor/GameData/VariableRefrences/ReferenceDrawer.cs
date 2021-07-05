using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.VariablesRefrences
{
    public abstract class ReferenceDrawer : PropertyDrawer
    {
        protected GUIStyle _popupStyle;
        protected SerializedProperty _referenceType;


        protected void BeginProperty(ref Rect position, SerializedProperty property, ref GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
        }

        protected void DrawPopup(Rect position)
        {
            Rect buttonRect = new Rect(position);
            
            int indent = EditorGUI.indentLevel;
            float popupWidth = _popupStyle.fixedWidth + _popupStyle.margin.right;
            float popupHeight = _popupStyle.fixedHeight + _popupStyle.margin.top;
            buttonRect.width = popupWidth + (indent * popupWidth);
            buttonRect.height = popupHeight;
            buttonRect.x += ((EditorGUIUtility.labelWidth - (indent * (popupWidth - 1))) - popupWidth);
            
            
            _referenceType.enumValueIndex = EditorGUI.Popup(buttonRect, _referenceType.enumValueIndex, 
                _referenceType.enumDisplayNames, _popupStyle);
        }
        
        protected void DrawProperty(Rect position, SerializedProperty baseProperty, params SerializedProperty[] properties)
        {
            DrawPopup(position);

            GUIContent label = new GUIContent(baseProperty.displayName);
            
            for (int i = 0; i < properties.Length; i++)
            {
                if (i == _referenceType.enumValueIndex)
                {
                    EditorGUI.PropertyField(position,  properties[i], label, true);
                }
            }
        }
        
        protected void EndProperty(SerializedProperty property)
        {
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.EndProperty();
        }

        protected void CacheReferenceType(SerializedProperty property, string path)
        {
            _referenceType = property.FindPropertyRelative(path);
        }
        
        protected void CachePopupStyle()
        {
            if (_popupStyle == null)
            {
                _popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                _popupStyle.imagePosition = ImagePosition.ImageOnly;
            }
        }
    }
}