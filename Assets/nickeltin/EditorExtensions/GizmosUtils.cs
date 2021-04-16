using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Editor
{
    public static class GizmosUtils
    {
        public static void DrawParabola(Vector3 from, Vector3 topPoint, Vector3 to, int segmentsCount, Color color)
        {
            Gizmos.color = color;
            float step = 1f / segmentsCount;
            for (int i = 0; i < segmentsCount; i++)
            {
                Vector3 pointA = from.Parabola(topPoint, to, step * i);
                Vector3 pointB = from.Parabola(topPoint, to, step * (i + 1));
                Gizmos.DrawLine(pointA, pointB);
            }
        }
    }
}