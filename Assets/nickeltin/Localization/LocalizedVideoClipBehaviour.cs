using UnityEngine;
using UnityEngine.Video;

namespace nickeltin.Localization
{
    [AddComponentMenu(ComponentMenuRoot + "Localized Video Clip")]
    public class LocalizedVideoClipBehaviour : LocalizedGenericAssetBehaviour<LocalizedVideoClip, VideoClip>
    {
        private void Reset()
        {
            m_Component = GetComponent<VideoPlayer>();
            if (m_Component)
            {
                m_Property = "clip";
            }
        }
    }
}