using System;
using UnityEngine;

namespace nickeltin.Other
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] m_rigidbodies;
        [SerializeField] private Collider[] m_colliders;
        [SerializeField] private Joint[] m_joints;


        private void OnValidate()
        {
            if (m_rigidbodies == null || m_rigidbodies.Length == 0)
            {
                m_rigidbodies = GetComponentsInChildren<Rigidbody>();
            }
            
            if (m_colliders == null || m_colliders.Length == 0)
            {
                m_colliders = GetComponentsInChildren<Collider>();
            }
            
            if (m_joints == null || m_joints.Length == 0)
            {
                m_joints = GetComponentsInChildren<Joint>();
            }
        }
        
        public void ForEachRigidbody(Action<Rigidbody> action) => ForEach(m_rigidbodies, action);
        public void ForEacCollider(Action<Collider> action) => ForEach(m_colliders, action);
        public void ForEachJoint(Action<Joint> action) => ForEach(m_joints, action);
        
        private void ForEach<T>(T[] from, Action<T> action)
        {
            for (var i = 0; i < from.Length; i++) action?.Invoke(from[i]);
        }

        public void SetActive(bool state)
        {
            ForEacCollider(c => c.enabled = state);
            ForEachRigidbody(r => r.isKinematic = !state);
        }

        [ContextMenu("Refresh references")]
        private void Refresh()
        {
            m_rigidbodies = GetComponentsInChildren<Rigidbody>();
            m_colliders = GetComponentsInChildren<Collider>();
            m_joints = GetComponentsInChildren<Joint>();
        }
    }
}