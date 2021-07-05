using nickeltin.Editor.PropertyDrawers;
using nickeltin.Runtime.Audio;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Audio
{
    [CustomPropertyDrawer(typeof(AudioEventData))]
    public class AudioEventDataDrawer : LayoutPropertyDrawer
    {
        protected override void DrawChildProperties(Rect position, SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_clips"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_volume"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_pitch"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_mixerGroup"));
        }
    }
}