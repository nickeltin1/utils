using System;
using nickeltin.Editor.Attributes;
using UnityEngine;

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
        public class Settings
        {
            public float x;
            public float y;
            public bool alignWithTargetRotation;
            [AllowNesting, HideIf("alignWithTargetRotation")] public Vector3 rotation;

            // public float xAspectRatio => x / y;
            // public float yAspectRatio => y / x;
            //
            // public float distanaceToCamera
            // {
            //     get => (x * xAspectRatio) + (y * yAspectRatio);
            //     set
            //     {
            //         float xRatio = xAspectRatio;
            //         float yRatio = yAspectRatio;
            //         x = xRatio * value;
            //         y = yRatio * value;
            //     }
            // }
        }

        public static Settings copiedSettings = new Settings();
        public new static Camera camera;
        public static Camera uiCamera;

        [SerializeField] private CameraTarget m_target;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Camera m_uiCamera;
        [SerializeField] private UpdateType m_updateType;
        [SerializeField, Range(0f,1f)] private float m_interpolationSpeed;
        [SerializeField] private Settings m_defaultSettings;

        public bool updatePosition { get; set; } = true;
        
        public float interpolationSpeed
        {
            get => m_interpolationSpeed;
            set => interpolationSpeed = value;
        }
        
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
                AlignPositionWithTarget();
                //bool alignWithTargetRot = m_defaultSettings.alignWithTargetRotation;
                m_defaultSettings = GetSettings();
                //m_defaultSettings.alignWithTargetRotation = alignWithTargetRot;
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
                transform.position = Vector3.Lerp(transform.position, m_target.transform.position, m_interpolationSpeed);
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, 
                    new Vector3(m_settings.x, m_settings.y, 0), interpolationSpeed);
            }

            if(m_settings.alignWithTargetRotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, m_target.transform.rotation, m_interpolationSpeed);
            }
        }

        public Settings GetSettings()
        {
            return new Settings
            {
                x = m_camera.transform.localPosition.x,
                y = m_camera.transform.localPosition.y,
                rotation = transform.eulerAngles,
                alignWithTargetRotation = m_defaultSettings.alignWithTargetRotation 
            };
        }


        public void SetSettings(Settings settings)
        {
            m_settings = settings;
            m_camera.transform.localPosition = new Vector3(m_settings.x, m_settings.y, 0);
            transform.rotation = Quaternion.Euler(m_settings.rotation);
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

        [ContextMenu("Align Position With Target")]
        private void AlignPositionWithTarget()
        {
            if (m_target != null)
            {
                transform.position = m_target.transform.position;
                if (m_target.overrideCameraSettings)
                {
                    SetSettings(m_target.settings);
                    if (m_target.settings.alignWithTargetRotation)
                        transform.rotation = m_target.transform.rotation;
                }
            }
            if (m_camera != null) m_camera.transform.LookAt(transform);
        }

#if UNITY_EDITOR
        protected override void CopySettings() => copiedSettings = GetSettings();

        protected override void PasteSettings() => SetSettings(copiedSettings);
#endif
    }
}
