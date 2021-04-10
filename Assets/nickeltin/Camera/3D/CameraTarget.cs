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

        private float m_initialFov;
        
        private void Awake() => m_initialFov = settings.fov;

        public void ChangeTarget() => onChange?.Invoke(this);

        public void ChangeFov(float newFov) => m_settings.fov = newFov;

        public void ResetFov() => ChangeFov(m_initialFov);

#if UNITY_EDITOR
        protected override void CopySettings() => CameraRig.copiedSettings = m_settings;

        protected override void PasteSettings() => m_settings = CameraRig.copiedSettings;
#endif
    }
}