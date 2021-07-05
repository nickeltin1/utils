using System;
using System.Collections.Generic;
using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.Utility;
using nickeltin.Extensions;
using nickeltin.Runtime.GameData.DataObjects;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.GameData.VariablesRefrences;
using nickeltin.Runtime.Singletons;
using UnityEngine;
using UnityEngine.Audio;

namespace nickeltin.Runtime.Audio
{
    [CreateAssetMenu(menuName = MenuPathsUtility.audioMenu + nameof(AudioManager))]
    public class AudioManager : SOSingleton<AudioManager>
    {
        [Serializable]
        private struct Settings
        {
            public DataObject<bool> channelEnabled;
            public string parameter;
            [MinMaxSlider(-100, 100)] public Vector2 range;
        }

        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup[] _groups;
        [SerializeField] private EventObject _refreshSettings;
        [SerializeField] private Settings[] _settings;
        [SerializeField] private AudioEvent[] _events;

        private Transform _audioSourcesParent;
        private Dictionary<AudioMixerGroup, AudioSource> _audioSources;
        private AudioSource _defaultSource;
        
        public override bool Initialize()
        {
            if(base.Initialize())
            {
                _audioSources = new Dictionary<AudioMixerGroup, AudioSource>();
                _audioSourcesParent = new GameObject("audio").transform;
                _defaultSource = _audioSourcesParent.gameObject.AddComponent<AudioSource>();
                DontDestroyOnLoad(_audioSourcesParent.gameObject);
                _groups.ForEach(group =>
                {
                    var audioSource = new GameObject($"{group.name}_AudioSource").AddComponent<AudioSource>();
                    audioSource.outputAudioMixerGroup = group;
                    _audioSources.Add(group, audioSource);
                    audioSource.transform.SetParent(_audioSourcesParent);
                });
                
                
                SubscribeForEvents();
                _refreshSettings.BindEvent(ApplyAllSettings);
                ApplyAllSettings();
                return true;
            }

            return false;
        }
        
        public override bool Destruct()
        {
            if (base.Destruct())
            {
                UnsubscribeFromEvents();
                _refreshSettings.UnbindEvent(ApplyAllSettings);
                return true;
            }

            return false;
        }

        private void ApplyAllSettings() => _settings.ForEach(ApplySettings);

        private void ApplySettings(Settings settings)
        {
            if (settings.channelEnabled != null)
            {
                _mixer.SetFloat(settings.parameter, settings.channelEnabled ? settings.range.y : settings.range.x);
            }
        }
        
        private void Play(AudioEventData audioData)
        {
            if (audioData.MixerGroup == null)
            {
                audioData.Play(_defaultSource);
                return;
            }

            if (!_audioSources.ContainsKey(audioData.MixerGroup))
            {
                throw new Exception($"{nameof(AudioMixerGroup)} {audioData.MixerGroup} not assigned in {name}");
            }
            
            audioData.Play(_audioSources[audioData.MixerGroup]);
        }

        private void SubscribeForEvents() => _events.ForEach(e => e.BindEvent(Play));

        private void UnsubscribeFromEvents() => _events.ForEach(e => e.UnbindEvent(Play));

#if UNITY_EDITOR
        [ContextMenu("Refresh AudioEvents"), Button("Refresh Events", EButtonEnableMode.Editor)]
        private void RefreshEventsList() => _events = Resources.FindObjectsOfTypeAll<AudioEvent>();

        [ContextMenu("Refresh AudioEvents", true)]
        private bool RefreshEvents_Validator() => !Application.isPlaying;
#endif
    }
}