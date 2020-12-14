using UnityEngine;

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
        
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        public static Vector3 Add(this Vector3 vector,  float? x = null, float? y = null, float? z = null)
        {
            return new Vector3((vector.x + x) ?? vector.x, (vector.y + y) ?? vector.y, (vector.z + z) ?? vector.z);
        }

        public static Vector3 Sub(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3((vector.x - x) ?? vector.x, (vector.y - y) ?? vector.y, (vector.z - z) ?? vector.z);
        }

        public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => to - from;

        public static Vector3 Div(this Vector3 vector, Vector3 by)
        {
            return new Vector3(vector.x / by.x, vector.y / by.y, vector.z / by.z);
        }

        public static Vector2 Div(this Vector2 vector, Vector2 by)
        {
            return new Vector2(vector.x / by.x, vector.y / by.y);
        }
        
        public static Vector3 Mult(this Vector3 x, float n) => x * n;
        
        public static Vector3 Mult(this Vector3 x, Vector3 n) => Vector3.Scale(x, n);
        
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
    }
}