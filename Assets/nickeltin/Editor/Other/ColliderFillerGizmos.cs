using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public static class ColliderFillerGizmos
    {
        [DrawGizmo(GizmoType.Active | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy, drawnType = typeof(ColliderFiller))]
        private static void DrawGizmos(ColliderFiller instance, GizmoType type)
        {
            if(instance.drawType == ColliderFiller.DrawType.DontDraw) return;

            if(instance.drawType == ColliderFiller.DrawType.OnlyWhenSelected 
               && type.HasFlag(GizmoType.NotInSelectionHierarchy)) return;

            Transform transform = instance.transform;

            if (!instance.color.Assignable) Gizmos.color = Color.magenta;
            else Gizmos.color = instance.color;

            switch (instance.collider)
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