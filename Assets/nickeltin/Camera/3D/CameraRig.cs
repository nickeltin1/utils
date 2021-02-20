using System;
using System.Collections;
using nickeltin.Editor.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace nickeltin.Cameras.TriDimensional
{
    [ExecuteInEditMode]
    public class CameraRig : CameraSystemObjectBase
    {
        [Serializable]
        public enum UpdateType
        {
            Update, LateUpdate, FixedUpdate
        }

        [Serializable]
        public struct Settings
        {
            public float x;
            public float y;
            public bool alignWithTargetRotation;
            [AllowNesting, HideIf("alignWithTargetRotation")] public Vector3 rotation;
        }
        
        [Serializable]
        private class InterpolationSettings
        {
            [Range(0, 1)] public float positionLerpSpeed = 0.5f;
            [Range(0, 1)] public float localPositionLerpSpeed = 0.5f;
            [Range(0, 1)] public float rotationLerpSpeed = 0.5f;
        }

        public static Settings copiedSettings = new Settings();
        public new static Camera camera;
        public static Camera uiCamera;

        [SerializeField] private CameraTarget m_target;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Camera m_uiCamera;
        [SerializeField] private UpdateType m_updateType;
        [SerializeField] private InterpolationSettings m_lerpSettings;
        [SerializeField, DisableIf("m_hasTarget")] private Settings m_defaultSettings;

        private bool m_hasTarget => m_target != null;
        
        public bool updatePosition { get; set; } = true;
        
        public bool shaking { get; private set; }

        public UpdateType updateType
        {
            get => m_updateType;
            set => m_updateType = value;
        }

        public float fov
        {
            get => m_camera.fieldOfView;
            set => m_camera.fieldOfView = value;
        }
        
        private Settings m_settings;
        private Quaternion m_targetedRotation;
        private Vector3 m_targetedCameraLocalPos;

        private void Awake()
        {
            m_settings = m_defaultSettings;
            ChangeTarget(m_target);
            
            camera = m_camera;
            uiCamera = m_uiCamera;
        }

        private void OnEnable() => CameraTarget.onChange += ChangeTarget;

        private void OnDisable() => CameraTarget.onChange -= ChangeTarget;

        private void OnDrawGizmos()
        {
            if (m_camera == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_camera.transform.position);
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (m_target == null || !m_target.overrideCameraSettings)
                {
                    ApplySettings_Editor(m_defaultSettings);
                }
                AlignPositionWithTarget_Editor();
            }
            
            if(m_updateType == UpdateType.Update) Update_Internal();
        }
        
        private void FixedUpdate()
        {
           if(m_updateType == UpdateType.FixedUpdate) Update_Internal();
        }

        private void LateUpdate()
        {
            if(m_updateType == UpdateType.LateUpdate) Update_Internal();
        }
        
        private void Update_Internal()
        {
            if( m_camera == null) return;
            
            if (!Application.isPlaying || m_target == null) return;
            
            m_camera.transform.LookAt(transform);
            
            if(updatePosition)
            {
                m_targetedCameraLocalPos = new Vector3(m_settings.x, m_settings.y, 0);
                
                transform.position = Vector3.Lerp(transform.position, m_target.transform.position, 
                    m_lerpSettings.positionLerpSpeed);
                
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, 
                    m_targetedCameraLocalPos, m_lerpSettings.localPositionLerpSpeed);
            }

            if(m_settings.alignWithTargetRotation)
            {
                m_targetedRotation = m_target.transform.rotation;
            }
            
            transform.rotation = Quaternion.Lerp(transform.rotation, m_targetedRotation, m_lerpSettings.rotationLerpSpeed);
        }

        public Settings GetSettings()
        {
            var settings = new Settings
            {
                rotation = transform.eulerAngles,
                alignWithTargetRotation = m_defaultSettings.alignWithTargetRotation 
            };

            if (m_camera != null)
            {
                settings.x = m_camera.transform.localPosition.x;
                settings.y = m_camera.transform.localPosition.y;
            }

            return settings;
        }


        public void SetSettings(Settings settings)
        {
            m_settings = settings;
            m_targetedRotation = Quaternion.Euler(m_settings.rotation);
        }

        public void ChangeTarget(CameraTarget target)
        {
            if (target != null)
            {
                if (target.overrideCameraSettings)
                {
                    SetSettings(target.settings);
                    m_target = target;
                }
                else SetSettings(m_defaultSettings);
            }
        }

        public void Shake(float t, float amplitude)
        {
            IEnumerator Shake()
            {
                shaking = true;
                for (float i = 0; i < t; i+=Time.deltaTime)
                {
                    Vector3 pos = new Vector3(m_settings.x, m_settings.y, 0) + (Random.insideUnitSphere * amplitude);
                    m_camera.transform.localPosition = pos;
                    yield return null;
                }

                shaking = false;
                SetSettings(m_settings);
            }

            StartCoroutine(Shake());
        }
        
        

        [ContextMenu("Align Position With Target")]
        private void AlignPositionWithTarget_Editor()
        {
            if (m_target != null)
            {
                transform.position = m_target.transform.position;
                if (m_target.overrideCameraSettings)
                {
                    ApplySettings_Editor(m_target.settings);
                    if (m_target.settings.alignWithTargetRotation)
                        transform.rotation = m_target.transform.rotation;
                }
            }
            if (m_camera != null) m_camera.transform.LookAt(transform);
        }

        private void ApplySettings_Editor(Settings settings)
        {
            m_camera.transform.localPosition = new Vector3(settings.x, settings.y, 0);
            transform.rotation = Quaternion.Euler(settings.rotation);;
        }

#if UNITY_EDITOR
        protected override void CopySettings() => copiedSettings = GetSettings();

        protected override void PasteSettings() => ApplySettings_Editor(copiedSettings);
#endif
    }
}
