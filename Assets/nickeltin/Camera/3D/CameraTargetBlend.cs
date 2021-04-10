using UnityEngine;

namespace nickeltin.Cameras.TriDimensional
{
    [RequireComponent(typeof(CameraTarget))]
    public class CameraTargetBlend : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField, Range(0,1)] private float m_targetX;
        [SerializeField, Range(0,1)] private float m_targetY;
        [SerializeField, Range(0,1)] private float m_targetZ;

        private void Update()
        {
            float x = Mathf.Lerp(transform.position.x, m_target.position.x, m_targetX);
            float y = Mathf.Lerp(transform.position.y, m_target.position.y, m_targetY);
            float z = Mathf.Lerp(transform.position.z, m_target.position.z, m_targetZ);
            
            transform.position = new Vector3(x, y, z);
        }
    }
}