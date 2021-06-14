using System;
using System.Collections.Generic;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Editor
{
    public static class GizmosUtils
    {
        public static void DrawParabola(Vector3 from, Vector3 topPoint, Vector3 to, int segmentsCount, 
            Action<Vector3> foreachPoint = null, Color? c = null)
        {
            DrawPath(f => from.Parabola(topPoint, to, f), 
                segmentsCount, foreachPoint, c);
        }

        public static void DrawPath(IReadOnlyList<Vector3> path, Action<Vector3> foreachPoint = null, 
            Color? c = null)
        {
            SetColor(c);
            
            for (int i = 0; i < path.Count; i++)
            {
                if (i + 1 < path.Count) Gizmos.DrawLine(path[i], path[i + 1]);
                foreachPoint?.Invoke(path[i]);
            }
        }

        public static void DrawPath(Func<float, Vector3> positionFunc, int segmentsCount, 
            Action<Vector3> foreachPoint = null, Color? c = null)
        {
            SetColor(c);
            float step = 1f / segmentsCount;
            for (int i = 0; i < segmentsCount; i++)
            {
                Vector3 pointA = positionFunc(step * i);
                Vector3 pointB = positionFunc(step * (i + 1));
                Gizmos.DrawLine(pointA, pointB);
                foreachPoint?.Invoke(pointA);
            }
        }
        
        public static void DrawSphere(Vector3 center, float r, Color? c = null)
        {
            SetColor(c);
            Gizmos.DrawSphere(center, r);
        }

        public static void DrawLine(Vector3 from, Vector3 to, Color? c = null)
        {
            SetColor(c);
            Gizmos.DrawLine(from, to);
        }

        public static void SetColor(Color? c) => Gizmos.color = c ?? Gizmos.color;
    }
}