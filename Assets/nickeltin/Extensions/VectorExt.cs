using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace nickeltin.Extensions
{
    public static class VectorExt
    {
        public static string ToString(this Vector3 vector, int digits)
        {
            return $"{vector.x.ToString("F"+digits)} " +
                   $"{vector.y.ToString("F"+digits)} " +
                   $"{vector.z.ToString("F"+digits)}";
        }
        
        public static Vector3 Set(this ref Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return vector = new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        public static Vector3 SetNoRef(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        
        public static Vector3 Add(this ref Vector3 vector,  float? x = null, float? y = null, float? z = null)
        {
            return vector = new Vector3((vector.x + x) ?? vector.x, (vector.y + y) ?? vector.y, (vector.z + z) ?? vector.z);
        }

        public static Vector3 AddNoRef(this Vector3 vector,  float? x = null, float? y = null, float? z = null)
        {
            return new Vector3((vector.x + x) ?? vector.x, (vector.y + y) ?? vector.y, (vector.z + z) ?? vector.z);
        }

        public static Vector3 Sub(this ref Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return vector = new Vector3((vector.x - x) ?? vector.x, (vector.y - y) ?? vector.y, (vector.z - z) ?? vector.z);
        }
        
        public static Vector3 SubNoRef(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3((vector.x - x) ?? vector.x, (vector.y - y) ?? vector.y, (vector.z - z) ?? vector.z);
        }
        
        public static Vector2 Sub(this ref Vector2 vector, float? x = null, float? y = null, float? z = null)
        {
            return vector = new Vector2((vector.x - x) ?? vector.x, (vector.y - y) ?? vector.y);
        }
        
        public static Vector2 SubNoRef(this Vector2 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector2((vector.x - x) ?? vector.x, (vector.y - y) ?? vector.y);
        }

        public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => to - from;

        public static Vector3 Div(this ref Vector3 vector, Vector3 by)
        {
            return vector = new Vector3(vector.x / by.x, vector.y / by.y, vector.z / by.z);
        }
        
        public static Vector3 DivNonRef(this Vector3 vector, Vector3 by)
        {
            return new Vector3(vector.x / by.x, vector.y / by.y, vector.z / by.z);
        }

        public static Vector2 Div(this ref Vector2 vector, Vector2 by)
        {
            return vector = new Vector2(vector.x / by.x, vector.y / by.y);
        }
        
        public static Vector2 DivNonRef(this Vector2 vector, Vector2 by)
        {
            return new Vector2(vector.x / by.x, vector.y / by.y);
        }
        
        public static Vector3 Mult(this ref Vector3 x, float n) => x *= n;
        public static Vector3 MultNonRef(this Vector3 x, float n) => x * n;
        
        public static Vector3 Mult(this ref Vector3 x, Vector3 n) => x = Vector3.Scale(x, n);
        public static Vector3 MultNonRef(this Vector3 x, Vector3 n) => Vector3.Scale(x, n);
        
        public static Vector3Int ToInt(this Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }

        public static float GetRandomValueBetweenAxis(this Vector2 vector)
        {
            return Random.Range(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
        
        public static Vector2 ToViewportPosition(this Vector3 vector, Camera cam)
        {
            return cam.WorldToViewportPoint(vector);
        }

        public static Vector2 ToLocalPositionInRect(this Vector3 vector, Camera cam, RectTransform rect)
        {
            Vector2 screenPoint = cam.WorldToScreenPoint(vector);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out screenPoint);
            return screenPoint;
        }

        public static Vector2 FromWorldToCanvasPosition(this Vector3 worldPos, Camera cam, Canvas canvas)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
            screenPoint -= canvas.pixelRect.size / 2;
            screenPoint *= canvas.scaleFactor;
            return screenPoint;
        }

        public static Vector3 RandomOffset(this Vector3 vector, Vector3 offsetRange)
        {
            return vector + Random.insideUnitSphere.MultNonRef(offsetRange);
        }
        
        public static Vector3 RandomOffset(this Vector3 vector, float offsetRange)
        {
            return vector + Random.insideUnitSphere.MultNonRef(offsetRange);
        }

        /// <summary>
        /// Clamps each axis within range
        /// </summary>
        /// <returns>Clamped vector</returns>
        public static Vector3 Clamp(this ref Vector3 vector, float min, float max)
        {
            return vector = new Vector3(Mathf.Clamp(vector.x, min, max), 
                Mathf.Clamp(vector.y, min, max), 
                Mathf.Clamp(vector.z, min, max));
        }

        /// <summary>
        /// Clamps each axis within range of [-1 - 1]
        /// </summary>
        public static Vector3 Clamp1(this ref Vector3 vector) => vector.Clamp(-1, 1);

        public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = float.Epsilon)
        {
            return (Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance);
        }

        public static Quaternion LookRotation(this Vector3 from, Vector3 to) => LookRotation(@from.DirectionTo(to));
        public static Quaternion LookRotation(this Vector3 direction) => Quaternion.LookRotation(direction);
    }
}