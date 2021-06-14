using System;
using UnityEngine;

namespace nickeltin.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {
        public new Collider collider;
        [SerializeField] private MonoBehaviour m_owner;

        public bool HasOwner => m_owner != null; 

        private void OnValidate()
        {
            if (collider == null) collider = GetComponent<Collider>();
        }

        public void SetOwner<T>(T owner) where T : MonoBehaviour
        {
            this.m_owner = owner;
        }
        
        public bool TryGetOwner<T>(out T owner)
        {
            if (HasOwner && this.m_owner is T requestedObject)
            {
                owner = requestedObject;
                return true;
            }

            owner = default;
            return false;
        }

        public static bool TryGetOwner<T>(GameObject other, out T owner)
        {
            if (other.TryGetComponent(out Hitbox hitbox) && hitbox.TryGetOwner(out owner))
            {
                return true;
            }
            
            owner = default;
            return false;
        }
    }
}