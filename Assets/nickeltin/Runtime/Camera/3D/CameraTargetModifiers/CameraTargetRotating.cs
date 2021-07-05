using UnityEngine;

namespace nickeltin.Runtime.Cameras.TriDimensional.CameraTargetModifiers
{
    public class CameraTargetRotating : CameraTargetModifier
    {
        [SerializeField] private Vector3 m_axis;
        [SerializeField] private float m_rotationSpeed;
        [SerializeField] private bool m_rotating = true;

        public bool rotating
        {
            get => m_rotating;
            set => m_rotating = value;
        }
        
        private void Update()
        {
            if (m_rotating)
            {
                cameraTarget.transform.rotation *= Quaternion.AngleAxis(m_rotationSpeed * Time.deltaTime, m_axis);
            }
        }
    }
}