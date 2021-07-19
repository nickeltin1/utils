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
            EditorGUILayout.PropertyField(property.FindPropertyRelative(AudioEventData.clips_prop_name));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(AudioEventData.volume_prop_name));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(AudioEventData.pitch_prop_name));
            EditorGUILayout.PropertyField(property.FindPropertyRelative(AudioEventData.mixer_group_prop_name));
        }
    }
}