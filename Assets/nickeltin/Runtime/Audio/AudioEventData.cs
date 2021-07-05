using System;
using nickeltin.Extensions;
using nickeltin.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace nickeltin.Runtime.Audio
{
    [Serializable]
    public class AudioEventData
    {
        [SerializeField] private AudioClip[] _clips;
        [SerializeField, MinMaxSlider(0, 2)] private Vector2 _volume = new Vector2(1,1);
        [SerializeField, MinMaxSlider(0, 2)] private Vector2 _pitch = new Vector2(1,1);
        [SerializeField] private AudioMixerGroup _mixerGroup;
        
        public bool HasClips => _clips != null && _clips.Length > 0;
        public AudioClip Clip => HasClips ? _clips.GetRandom() : null;
        public float Volume => _volume.GetRandomValueBetweenAxis();
        public float Pitch => _pitch.GetRandomValueBetweenAxis();
        public AudioMixerGroup MixerGroup => _mixerGroup;
        
        public void Play(AudioSource source)
        {
            if(!HasClips) return;

            source.clip = Clip;
            source.volume = Volume;
            source.pitch = Pitch;
            source.Play();
        }
    }
}