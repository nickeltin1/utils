using nickeltin.GameData.References;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor
{
    [CustomPropertyDrawer(typeof(VarRef<>), true)]
    public sealed class VariableReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, ref label);
            
            CacheReferenceType(property, "m_referenceType");
            
            SerializedProperty constantValue = property.FindPropertyRelative("m_constantValue");
            SerializedProperty dataObject = property.FindPropertyRelative("m_dataObject");
            SerializedProperty globalVariable = property.FindPropertyRelative("m_globalVariable");
            
            DrawProperty(position, property,constantValue, dataObject, globalVariable);

            EndProperty(property);
        }
    }
}