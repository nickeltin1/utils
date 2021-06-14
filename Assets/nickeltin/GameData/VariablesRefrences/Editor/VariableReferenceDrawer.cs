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
            
            CacheReferenceType(property, "_referenceType");
            
            SerializedProperty constantValue = property.FindPropertyRelative("_constantValue");
            SerializedProperty dataObject = property.FindPropertyRelative("_dataObject");
            SerializedProperty globalVariable = property.FindPropertyRelative("_globalVariable");
            
            DrawProperty(position, property,constantValue, dataObject, globalVariable);

            EndProperty(property);
        }
    }
}