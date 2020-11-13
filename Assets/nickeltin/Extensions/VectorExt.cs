using UnityEngine;

namespace nickeltin.Extensions.Vector
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

        public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => to - from;

        public static Vector3 Div(this Vector3 vector, Vector3 by)
        {
            return new Vector3(vector.x / by.x, vector.y / by.y, vector.z / by.z);
        }
        
        public static Vector3 Mult(this Vector3 x, float n) => x * n;
        
        public static Vector3 Mult(this Vector3 x, Vector3 n) => Vector3.Scale(x, n);
        
        public static Vector3Int ToInt(this Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }
    }
}