using UnityEngine;

namespace nickeltin.Runtime.Cameras.TriDimensional.CameraTargetModifiers
{
    public class CameraTargetDynamic : CameraTargetModifier
    {
        [SerializeField] private float m_maxOffsetMultiplier = 100;
        [SerializeField] private float m_dependantValueMin = 1;
        [SerializeField] private float m_dependantValueMax = 100;
        
        private Vector2 m_initOffset;

        public void SetDependantRange(float min, float max)
        {
            m_dependantValueMin = min;
            m_dependantValueMax = max;
        }

        protected override void Awake()
        {
            base.Awake();
            m_initOffset = cameraTarget.settings.offset;
        }

        public void UpdateOffset(float dependentValue)
        {
            var settings = cameraTarget.settings;
            
            settings.offset = Vector2.Lerp(m_initOffset, m_initOffset * m_maxOffsetMultiplier,
                Mathf.InverseLerp(m_dependantValueMin, m_dependantValueMax, dependentValue));
            
            cameraTarget.settings = settings;
        }
    }
}