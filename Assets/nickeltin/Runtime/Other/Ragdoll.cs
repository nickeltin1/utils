using System;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private bool _autoCacheReferences = true;
        [SerializeField] private bool _disableAtStart = true;
        [SerializeField] private Rigidbody _mainRigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody[] _rigidbodies;
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private Joint[] _joints;

        public Rigidbody mainRigidbody { get => _mainRigidbody; set => _mainRigidbody = value; }


        private void OnValidate() { if(_autoCacheReferences) Refresh(); }

        private void Start()
        {
            if(_disableAtStart) SetActive(false);
        }

        public void ForEachRigidbody(Action<Rigidbody> action) => ForEach(_rigidbodies, action);
        public void ForEacCollider(Action<Collider> action) => ForEach(_colliders, action);
        public void ForEachJoint(Action<Joint> action) => ForEach(_joints, action);
        
        private void ForEach<T>(T[] from, Action<T> action) => from.ForEach(item=> action?.Invoke(item)); 

        public void SetActive(bool state)
        {
            if (_animator != null) _animator.enabled = !state;
            ForEacCollider(c => c.enabled = state);
            ForEachRigidbody(r => r.isKinematic = !state);
        }

        [ContextMenu("Refresh references")]
        private void Refresh()
        {
            ComponentExt.CacheInChildrens(ref _rigidbodies, gameObject);
            ComponentExt.CacheInChildrens(ref _colliders, gameObject);
            ComponentExt.CacheInChildrens(ref _joints, gameObject);
            ComponentExt.Cache(ref _animator, gameObject);
        }
    }
}