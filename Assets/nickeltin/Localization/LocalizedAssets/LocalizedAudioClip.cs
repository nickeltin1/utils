using System;
using UnityEngine;

namespace nickeltin.Localization
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizedAudioClip", menuName = "Localization/AudioClip")]
    public class LocalizedAudioClip : LocalizedAsset<AudioClip>
    {
        [Serializable]
        private class AudioClipLocaleItem : LocaleItem<AudioClip> { };

        [SerializeField]
        private AudioClipLocaleItem[] m_LocaleItems = new AudioClipLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
