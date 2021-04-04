using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Types.Editor
{
    [CustomPropertyDrawer(typeof(Event), false)]
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