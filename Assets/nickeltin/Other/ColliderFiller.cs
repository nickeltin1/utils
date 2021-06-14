using System;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Other
{
    [RequireComponent(typeof(Collider))]
    public class ColliderFiller : MonoBehaviour
    {
        [SerializeField] private Collider m_collider;
        public Color color;


        private void OnValidate()
        {
            if (m_collider == null) m_collider = GetComponent<Collider>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
           
            switch (m_collider)
            {
                case BoxCollider box:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                    Gizmos.DrawCube(box.center, box.size);
                    break;
                case SphereCollider sphere:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    Vector3 scale = transform.lossyScale;
                    scale.Abs();
                    float maxScale = scale.x;
                    if (scale.y > maxScale) maxScale = scale.y;
                    if (scale.z > maxScale) maxScale = scale.z;
                    Gizmos.DrawSphere(sphere.center, sphere.radius * maxScale);
                    break;
                case CapsuleCollider capsule:
                    //TODO:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                    float halfHeight = (capsule.height / 2) - capsule.radius;
                    Vector3 offset = new Vector3(halfHeight, halfHeight, halfHeight);
                    if (capsule.direction == 0) offset.Set(y: 0, z: 0);
                    else if (capsule.direction == 1) offset.Set(x: 0, z: 0);
                    else if (capsule.direction == 2) offset.Set(x: 0, y: 0);
                    Gizmos.DrawSphere(offset, capsule.radius);
                    Gizmos.DrawSphere(-offset, capsule.radius);
                    break;
            }
        }
    }
}