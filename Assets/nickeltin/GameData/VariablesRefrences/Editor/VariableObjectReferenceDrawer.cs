using nickeltin.GameData.Editor;
using UnityEditor;
using UnityEngine;


namespace nickeltin.UI.Editor
{
    [CustomPropertyDrawer(typeof(VarObjRef<>), true)]
    public class VariableObjectReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, ref label);
            
            CacheReferenceType(property, "m_sourceType");
            
            SerializedProperty dataObject = property.FindPropertyRelative("m_dataObjectSource");
            SerializedProperty globalVariable = property.FindPropertyRelative("m_globalVariableSource");
            
            DrawProperty(position, dataObject, globalVariable);
            
            EndProperty(property);
        }
    }
}
