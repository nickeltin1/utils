using nickeltin.Runtime.Audio;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Audio
{
    [CustomEditor(typeof(AudioEvent))]
    public class AudioEventDrawer : UnityEditor.Editor
    {
        private static AudioSource previewSource;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if (GUILayout.Button("Preview"))
            {
                if (previewSource == null)
                {
                    previewSource = EditorUtility.CreateGameObjectWithHideFlags("audio_preview", HideFlags.HideAndDontSave)
                        .AddComponent<AudioSource>();
                }
            
                ((AudioEvent)target).Play(previewSource);
            }
        }
    }
}