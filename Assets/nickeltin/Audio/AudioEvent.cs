using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Audio
{
    [CreateAssetMenu(menuName = "Audio/AudioEvent")]
    public class AudioEvent : ScriptableObject
    {
        private static AudioSource previewSource;
        
        [SerializeField] private AudioClip[] m_clips;
        [SerializeField, MinMaxSlider(0, 2)] private Vector2 m_volume;
        [SerializeField, MinMaxSlider(0, 2)] private Vector2 m_pitch;
        
        public bool hasClips => m_clips != null && m_clips.Length > 0;
        /// <summary>
        /// Returns random clip if has any, otherwise returns null
        /// </summary>
        public AudioClip clip => hasClips ? m_clips.GetRandom() : null;
        /// <summary>
        /// Gets random volume within 0 - 2 range
        /// </summary>
        public float volume => m_volume.GetRandomValueBetweenAxis();
        /// <summary>
        /// Gets random pitch within 0 - 2 range
        /// </summary>
        public float pitch => m_pitch.GetRandomValueBetweenAxis();
        
        
        public void Play(AudioSource source)
        {
            if(!hasClips) return;

            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
        
        
#if UNITY_EDITOR
        [Button("Preview")]        
        private void Play_Editor()
        {
            if (previewSource == null)
            {
                previewSource = EditorUtility.CreateGameObjectWithHideFlags("audio_preview", HideFlags.HideAndDontSave)
                    .AddComponent<AudioSource>();
            }
            
            Play(previewSource);
        }
#endif
    }
}