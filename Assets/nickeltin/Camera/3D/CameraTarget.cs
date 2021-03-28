using System;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Cameras.TriDimensional
{
    public class CameraTarget : CameraSystemObjectBase
    {
        public static event Action<CameraTarget> onChange;

        [SerializeField] private bool m_overrideCameraSettings;
        [SerializeField, ShowIf("m_overrideCameraSettings")] private CameraRig.Settings m_settings;

        public bool overrideCameraSettings => m_overrideCameraSettings;
        public CameraRig.Settings settings => m_settings;
        
        public void ChangeTarget() => onChange?.Invoke(this);

#if UNITY_EDITOR
        protected override void CopySettings() => CameraRig.copiedSettings = m_settings;

        protected override void PasteSettings() => m_settings = CameraRig.copiedSettings;
#endif
    }
}