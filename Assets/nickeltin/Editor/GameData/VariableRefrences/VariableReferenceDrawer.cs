using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.VariablesRefrences
{
    [CustomPropertyDrawer(typeof(VariableRef<>), true)]
    public sealed class VariableReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, label);
            
            CacheReferenceType(property, VariableRef<int>.ref_type_prop_name);
            
            SerializedProperty constantValue = property.FindPropertyRelative(VariableRef<int>.const_value_prop_name);
            SerializedProperty dataObject = property.FindPropertyRelative(VariableRef<int>.data_obj_prop_name);
            
            DrawProperty(position, label,constantValue, dataObject);

            EndProperty(property);
        }
    }
}