using nickeltin.Experimental.GlobalVariables.Types;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;
using Event = nickeltin.Experimental.GlobalVariables.Types.Event;

namespace nickeltin.Experimental.GlobalVariables.Editor
{
    [CustomPropertyDrawer(typeof(Event))]
    public class EventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawButton(position, property);
        }

        protected void DrawButton(Rect position, SerializedProperty property)
        {
            GUI.enabled = Application.isPlaying;
            
            if (GUI.Button(position, "Invoke"))
            {
                (property.GetTargetObject() as EventBase)?.Invoke_Editor();
            }

            GUI.enabled = true;
        }
    }
}