using nickeltin.Experimental.GlobalVariables.Types;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Experimental.GlobalVariables.Editor
{
    [CustomPropertyDrawer(typeof(Event<>), true)]
    public class GenericEventDrawer : EventDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float halfWidth =  position.width / 2;
            position.width = halfWidth;
            SerializedProperty invokeData = property.FindPropertyRelative("invokeData");
            EditorGUI.PropertyField(position, invokeData, GUIContent.none);
            position.x += halfWidth;
            DrawButton(position,property);
        }
    }
}