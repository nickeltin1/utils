using System;
using System.Collections.Generic;
using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.ObjectPooling;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public class TrajectoryDrawer : MonoBehaviour
    {
        private enum RenderType
        {
            Line, Dotted, Gizmos
        }

        [SerializeField, Range(4, 100)] private int m_pointsCount = 20;
        public Vector3 force;
        [SerializeField] private RenderType m_renderType;
        private bool m_lineRenderTypeValidator => m_renderType == RenderType.Line;
        [SerializeField, ShowIf("m_lineRenderTypeValidator")] private LineRenderer m_renderer;
        private bool m_dottedRenderTypeValidator => m_renderType == RenderType.Dotted;
        [SerializeField, ShowIf("m_dottedRenderTypeValidator")] private Transform m_pointPrefab;
        
        private bool m_gizmosRenderTypeValidator => m_renderType == RenderType.Gizmos;

        [SerializeField, ShowIf("m_gizmosRenderTypeValidator")] private Rigidbody m_rigidbody;
        [SerializeField, ShowIf("m_gizmosRenderTypeValidator")] 
        private bool m_useForwardForce;
        
        [SerializeField, ShowIf(EConditionOperator.And,"m_gizmosRenderTypeValidator", "m_useForwardForce")] 
        private Vector3 m_forwardRotationOffset;
        [SerializeField, ShowIf(EConditionOperator.And,"m_gizmosRenderTypeValidator", "m_useForwardForce")] 
        private float m_forwardForce;
        [SerializeField, ShowIf(EConditionOperator.And,"m_gizmosRenderTypeValidator", "m_useForwardForce"), Range(-1,1)] 
        private float m_upMod;
        [SerializeField, ShowIf("m_gizmosRenderTypeValidator")] private Color m_gizmosColor = Color.red;
        
        private bool m_updatePostion = true;
        private Vector3 m_rememberedPosition;
        private Pool m_dots; 
        private readonly List<Vector3> m_points = new List<Vector3>();

        [HideInInspector] public bool draw = true;
        
        private void OnDrawGizmos()
        {
            if (m_gizmosRenderTypeValidator && m_rigidbody != null && draw)
            {
                if (m_useForwardForce)
                {
                    force = (transform.forward + (Vector3.up * m_upMod)) * m_forwardForce;
                    force = Matrix4x4.Rotate(Quaternion.Euler(m_forwardRotationOffset)).MultiplyVector(force);
                }
                
                CalculateTrajectory(m_rigidbody);

                Gizmos.color = m_gizmosColor;
                for (int i = 0; i < m_points.Count; i++)
                {
                    if (i + 1 < m_points.Count) Gizmos.DrawLine(m_points[i], m_points[i + 1]);
                    else if (i - 1 > 0)
                    {
                        Gizmos.DrawLine(m_points[i-1], m_points[i]);
                        Gizmos.DrawSphere(m_points[i], 0.1f);
                    }
                }
            }
        }
        
        private void Start()
        {
            if (m_pointPrefab != null)
            {
                m_dots = new Pool(m_pointPrefab);
            }
        }
        
        public void FreezeStartPosition()
        {
            m_updatePostion = false;
            m_rememberedPosition = transform.position;
        }
        
        public void StartTrajcetory() => m_renderer.enabled = true;

        public void EndTrajectory()
        {
            m_renderer.enabled = false;
            if (!m_lineRenderTypeValidator)
            {
                foreach (var dot in m_dots.Items)
                {
                    dot.gameObject.SetActive(false);
                }
            }
        }

        public void UpdateTrajcetory(Rigidbody rigidbody)
        {
            if(!draw) return;
            
            CalculateTrajectory(rigidbody);
            
            if (m_lineRenderTypeValidator)
            {
                m_renderer.positionCount = m_points.Count;
                m_renderer.SetPositions(m_points.ToArray());
            }
            else
            {
                for (int i = 0; i < m_points.Count; i++)
                {
                    var dot = m_dots.Get();
                    dot.position = m_points[i];
                }
                m_dots.ReturnAllObjectsToPool(false, true);
            }
        }

        private void CalculateTrajectory(Rigidbody rigidbody)
        {
            Vector3 velocity = (force / rigidbody.mass) * Time.fixedDeltaTime;

            float flightDuration = (2 * velocity.y) / Physics.gravity.y;

            float stepTime = flightDuration / m_pointsCount;
            
            m_points.Clear();

            for (int i = 0; i < m_pointsCount; i++)
            {
                float stepTimePassed = stepTime * i;
                
                Vector3 movementVector = new Vector3(
                    velocity.x * stepTimePassed,
                    velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                    velocity.z * stepTimePassed
                );
                
                m_points.Add(-movementVector + (m_updatePostion ? transform.position : m_rememberedPosition));
            }
        }
    }
}