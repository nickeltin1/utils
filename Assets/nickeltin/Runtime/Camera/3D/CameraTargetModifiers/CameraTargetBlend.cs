using UnityEngine;

namespace nickeltin.Runtime.Cameras.TriDimensional.CameraTargetModifiers
{
    public class CameraTargetBlend : CameraTargetModifier
    {
        public Transform target;
        [SerializeField] private Space m_space;
        [SerializeField, Range(0,1)] private float m_targetX;
        [SerializeField, Range(0,1)] private float m_targetY;
        [SerializeField, Range(0,1)] private float m_targetZ;
        
        private void Update()
        {
            if(target == null) return;

            if (m_space == Space.World)
            {
                float x = Mathf.Lerp(transform.position.x, target.position.x, m_targetX);
                float y = Mathf.Lerp(transform.position.y, target.position.y, m_targetY);
                float z = Mathf.Lerp(transform.position.z, target.position.z, m_targetZ);
                
                transform.position = new Vector3(x, y, z);
            }
            else
            {
                float x = Mathf.Lerp(transform.localPosition.x, target.localPosition.x, m_targetX);
                float y = Mathf.Lerp(transform.localPosition.y, target.localPosition.y, m_targetY);
                float z = Mathf.Lerp(transform.localPosition.z, target.localPosition.z, m_targetZ);
                
                transform.localPosition = new Vector3(x, y, z);
            }
        }
    }
}