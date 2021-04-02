using nickeltin.Editor.PropertyDrawers;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Events.Editor
{
    [CustomPropertyDrawer(typeof(EventReference<>))]
    public class EventReferenceDrawer : GenericObjectDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
        }
    }
}