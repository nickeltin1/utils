using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEditor;
using UnityEngine;


namespace nickeltin.Editor.GameData.VariablesRefrences
{
    [CustomPropertyDrawer(typeof(VarObjRef<>), true)]
    public class VariableObjectReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, ref label);
            
            CacheReferenceType(property, "_sourceType");
            
            SerializedProperty dataObject = property.FindPropertyRelative("_dataObjectSource");
            SerializedProperty globalVariable = property.FindPropertyRelative("_globalVariableSource");
            
            DrawProperty(position, property, dataObject, globalVariable);
            
            EndProperty(property);
        }
    }
}
