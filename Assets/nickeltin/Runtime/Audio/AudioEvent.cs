using System.Collections.Generic;
using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using UnityEngine;

namespace nickeltin.Runtime.Audio
{
    [CreateAssetMenu(menuName = MenuPathsUtility.audioMenu + nameof(AudioEvent))]
    public class AudioEvent : EventObject<AudioEventData>
    {
        public void Play(AudioSource source) => _event.invokeData.Play(source);

        public void SetClips(IEnumerable<AudioClip> clips) => _event.invokeData.SetClips(clips);
    }
}