using nickeltin.GameData.DataObjects;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor
{
    [CustomPropertyDrawer(typeof(EventReferenceBase), true)]
    public sealed class EventReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, ref label);
            
            CacheReferenceType(property, "m_referenceType");
            
            SerializedProperty eventObject = property.FindPropertyRelative("m_eventObject");
            SerializedProperty gloablEvent = property.FindPropertyRelative("m_globalEvent");

            DrawProperty(position, property,eventObject, gloablEvent);

            EndProperty(property);
        }
    }
}