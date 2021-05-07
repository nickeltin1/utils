using System;
using System.Collections;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
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
            public Vector2 offset;
            public bool alignWithTargetRotation;
            [AllowNesting, HideIf("alignWithTargetRotation")] public Vector3 rotation;
            public float fov;
        }
        
        [Serializable]
        private class InterpolationSettings
        {
            public float positionLerpSpeed = 100f;
            public float localPositionLerpSpeed = 100f;
            public float rotationLerpSpeed = 100f;
            public float fovLerpSpeed = 100f;
        }

        public static Settings copiedSettings; 
        public new static Camera camera { get; private set; }
        public static Camera uiCamera { get; private set; }
        public static CameraRig current { get; private set; }

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
        
        
        private Settings m_settings;
        private Quaternion m_targetedRotation;
        private Vector3 m_targetedCameraLocalPos;

        private void Awake()
        {
            current = this;
            
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
            
            if(m_updateType == UpdateType.Update) Update_Internal(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
           if(m_updateType == UpdateType.FixedUpdate) Update_Internal(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if(m_updateType == UpdateType.LateUpdate) Update_Internal(Time.deltaTime);
        }
        
        private void Update_Internal(float timeStep)
        {
            if( m_camera == null) return;
            
            if (!Application.isPlaying || m_target == null) return;

            m_settings = m_target != null ? m_target.settings : m_defaultSettings;
                
            m_camera.transform.LookAt(transform);

            if(updatePosition)
            {
                m_targetedCameraLocalPos = m_settings.offset;
                
                transform.position = Vector3.Lerp(transform.position, m_target.transform.position, 
                    m_lerpSettings.positionLerpSpeed * timeStep);

                //if (!shaking)
                //{
                    m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, 
                        m_targetedCameraLocalPos, m_lerpSettings.localPositionLerpSpeed * timeStep);
                //}
            }

            if(m_settings.alignWithTargetRotation)
            {
                m_targetedRotation = m_target.transform.rotation;
            }
            else
            {
                m_targetedRotation = Quaternion.Euler(m_target.settings.rotation);
            }
            
            transform.rotation = Quaternion.Lerp(transform.rotation, m_targetedRotation, 
                m_lerpSettings.rotationLerpSpeed * timeStep);

            if (m_camera != null)
            {
                m_camera.fieldOfView =  Mathf.Lerp(m_camera.fieldOfView, m_settings.fov, m_lerpSettings.fovLerpSpeed * timeStep);
            }
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
                settings.offset = new Vector2(m_camera.transform.localPosition.x, m_camera.transform.localPosition.y);
            }

            return settings;
        }


        public void SetSettings(Settings settings)
        {
            m_settings = settings;
            if(!settings.alignWithTargetRotation) m_targetedRotation = Quaternion.Euler(m_settings.rotation);
        }

        public void ChangeTarget(CameraTarget target)
        {
            if (target != null)
            {
                if (target.overrideCameraSettings) SetSettings(target.settings);
                else SetSettings(m_defaultSettings);
                m_target = target;
            }
        }

        public void Shake(float t, float amplitude)
        {
            IEnumerator Shake()
            {
                shaking = true;
                for (float i = 0; i < t; i+=Time.deltaTime)
                {
                    Vector3 pos = m_settings.offset.ToVector3(0) + (Random.insideUnitSphere * amplitude);
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
            if (m_camera != null)
            {
                m_camera.transform.localPosition = settings.offset.ToVector3(0);
                m_camera.fieldOfView = settings.fov;
            }
            transform.rotation = Quaternion.Euler(settings.rotation);
        }

#if UNITY_EDITOR
        protected override void CopySettings() => copiedSettings = GetSettings();

        protected override void PasteSettings()
        {
            m_defaultSettings = copiedSettings;
            ApplySettings_Editor(m_defaultSettings);
        }
#endif
    }
}
