using UnityEngine;

namespace nickeltin.Cameras.TriDimensional.CameraTargetModifiers
{
    public class CameraTargetBlend : CameraTargetModifier
    {
        public Transform target;
        [SerializeField, Range(0,1)] private float m_targetX;
        [SerializeField, Range(0,1)] private float m_targetY;
        [SerializeField, Range(0,1)] private float m_targetZ;
        
        private void Update()
        {
            if(target == null) return;

            float x = Mathf.Lerp(transform.position.x, target.position.x, m_targetX);
            float y = Mathf.Lerp(transform.position.y, target.position.y, m_targetY);
            float z = Mathf.Lerp(transform.position.z, target.position.z, m_targetZ);
            
            transform.position = new Vector3(x, y, z);
        }
    }
}