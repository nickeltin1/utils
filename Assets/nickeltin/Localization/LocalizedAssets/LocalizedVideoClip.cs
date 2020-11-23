using System;
using UnityEngine;
using UnityEngine.Video;

namespace nickeltin.Localization
{
    [CreateAssetMenu(fileName = "LocalizedVideoClip", menuName = "Localization/VideoClip")]
    public class LocalizedVideoClip : LocalizedAsset<VideoClip>
    {
        [Serializable]
        private class VideoClipLocaleItem : LocaleItem<VideoClip> { };

        [SerializeField]
        private VideoClipLocaleItem[] m_LocaleItems = new VideoClipLocaleItem[1];

        public override LocaleItemBase[] LocaleItems { get { return m_LocaleItems; } }
    }
}
