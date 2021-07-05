using nickeltin.Editor.Extensions;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.GameData.GlobalVariables;
using UnityEditor;
using UnityEngine;
using Event = nickeltin.Runtime.GameData.Events.Event;

namespace nickeltin.Editor.GameData.Events
{
    [CustomPropertyDrawer(typeof(Event), false)]
    public class EventDrawer : PropertyDrawer
    {
        protected bool VerifyContext(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject is MonoBehaviour)
            {
                float width = position.width;
                float lableWidth = EditorGUIUtility.labelWidth;
                float spacing = EditorGUIUtility.standardVerticalSpacing;
                position.width = lableWidth;
                EditorGUI.LabelField(position, label);

                position.x += lableWidth + spacing;
                position.width = width - lableWidth - spacing;
                EditorGUI.HelpBox(position, 
                    $"Do not use {nameof(Event)} explicitly, use {nameof(EventObject)} or " +
                    $"{nameof(EventRegistry)} for events implementation", 
                    MessageType.Warning);
                return false;
            }

            return true;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(!VerifyContext(position, property, label)) return;
            
            EditorGUI.LabelField(position, label);
            DrawButton(GetFullButtonRect(position), property);
        }

        protected Rect GetFullButtonRect(Rect source)
        {
            var rect = new Rect(source);
            rect.width -= EditorGUIUtility.labelWidth;
            rect.x += EditorGUIUtility.labelWidth;
            return rect;
        }

        protected void DrawButton(Rect position, SerializedProperty property)
        {
            GUI.enabled = Application.isPlaying;
            
            if (GUI.Button(position, "Invoke"))
            {
                (property.GetTargetObject() as EventBase)?.InvokeWithDefaultData();
            }

            GUI.enabled = true;
        }
    }
}