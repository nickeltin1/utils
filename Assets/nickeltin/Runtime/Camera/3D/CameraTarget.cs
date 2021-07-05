using System;
using nickeltin.Extensions.Attributes;
using UnityEngine;

namespace nickeltin.Runtime.Cameras.TriDimensional
{
    public class CameraTarget : CameraSystemObjectBase
    {
        public static event Action<CameraTarget> onChange;

        [SerializeField] private bool m_overrideCameraSettings;
        [SerializeField, ShowIf("m_overrideCameraSettings")] public CameraRig.Settings settings;

        public bool overrideCameraSettings => m_overrideCameraSettings; 
        

        private float m_initialFov;
        
        private void Awake() => m_initialFov = settings.fov;

        public void ChangeTarget() => onChange?.Invoke(this);

        public void ChangeFov(float newFov) => settings.fov = newFov;

        public void ResetFov() => ChangeFov(m_initialFov);

#if UNITY_EDITOR
        protected override void CopySettings() => CameraRig.copiedSettings = settings;

        protected override void PasteSettings() => settings = CameraRig.copiedSettings;
#endif
    }
}