using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor
{
    public abstract class ReferenceDrawer : PropertyDrawer
    {
        protected GUIStyle m_popupStyle;

        protected SerializedProperty m_referenceType;

        private int m_indent;

        protected void BeginProperty(ref Rect position, SerializedProperty property, ref GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            m_indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            EditorGUI.BeginChangeCheck();
        }

        protected void DrawPopup(ref Rect position)
        {
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += m_popupStyle.margin.top;
            buttonRect.width = m_popupStyle.fixedWidth + m_popupStyle.margin.right;
            position.xMin = buttonRect.xMax;
            
            m_referenceType.enumValueIndex = EditorGUI.Popup(buttonRect, m_referenceType.enumValueIndex, 
                m_referenceType.enumDisplayNames, m_popupStyle);
        }
        
        protected void DrawProperty(Rect position, params SerializedProperty[] properties)
        {
            if (Application.isPlaying) GUI.enabled = false;
            
            DrawPopup(ref position);
            
            for (int i = 0; i < properties.Length; i++)
            {
                if (i == m_referenceType.enumValueIndex)
                {
                    EditorGUI.PropertyField(position, properties[i], GUIContent.none);
                }
            }
            
            GUI.enabled = true;
        }
        
        protected void EndProperty(SerializedProperty property)
        {
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = m_indent;
            EditorGUI.EndProperty();
        }

        protected void CacheReferenceType(SerializedProperty property, string path)
        {
            m_referenceType = property.FindPropertyRelative(path);
        }
        
        protected void CachePopupStyle()
        {
            if (m_popupStyle == null)
            {
                m_popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                m_popupStyle.imagePosition = ImagePosition.ImageOnly;
            }
        }
    }
}