using System;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Other
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private bool _autoCacheReferences = true;
        [SerializeField] private Rigidbody[] m_rigidbodies;
        [SerializeField] private Collider[] m_colliders;
        [SerializeField] private Joint[] m_joints;


        private void OnValidate() { if(_autoCacheReferences) Refresh(); }
        
        public void ForEachRigidbody(Action<Rigidbody> action) => ForEach(m_rigidbodies, action);
        public void ForEacCollider(Action<Collider> action) => ForEach(m_colliders, action);
        public void ForEachJoint(Action<Joint> action) => ForEach(m_joints, action);
        
        private void ForEach<T>(T[] from, Action<T> action) => from.ForEach(item=> action?.Invoke(item)); 

        public void SetActive(bool state)
        {
            ForEacCollider(c => c.enabled = state);
            ForEachRigidbody(r => r.isKinematic = !state);
        }

        [ContextMenu("Refresh references")]
        private void Refresh()
        {
            ComponentExt.CacheInChildrens(ref m_rigidbodies, gameObject);
            ComponentExt.CacheInChildrens(ref m_colliders, gameObject);
            ComponentExt.CacheInChildrens(ref m_joints, gameObject);
        }
    }
}