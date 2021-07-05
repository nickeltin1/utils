using UnityEngine;

namespace nickeltin.Extensions
{
    public static class QuaternionExt
    {
        public static Quaternion Lerp(this ref Quaternion from, Quaternion to,  float t)
        {
            return from = Quaternion.Lerp(from, to, t);
        }

        public static Quaternion LerpNoRef(this Quaternion from, Quaternion to,  float t)
        {
            return Quaternion.Lerp(from, to, t);
        }
        
        public static Quaternion SlerpNoRef(this Quaternion from, Quaternion to,  float t)
        {
            return Quaternion.Slerp(from, to, t);
        }

        public static Quaternion Set(this ref Quaternion quaternion, float? x = null, float? y = null, float? z = null)
        {
            quaternion.Set(x ?? quaternion.x, y ?? quaternion.y, z ?? quaternion.z, quaternion.w);
            return quaternion;
        }
        
        public static Quaternion SetNoRef(this Quaternion quaternion, float? x = null, float? y = null, float? z = null)
        {
            return new Quaternion(x ?? quaternion.x, y ?? quaternion.y, z ?? quaternion.z, quaternion.w);
        }

        public static Quaternion Clamp(this ref Quaternion quaternion, Vector3 min, Vector3 max)
        {
            quaternion.x /= quaternion.w;
            quaternion.y /= quaternion.w;
            quaternion.z /= quaternion.w;
            quaternion.w = 1.0f;
 
            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.x);
            angleX = Mathf.Clamp(angleX, min.x, max.x);
            quaternion.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
 
            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.y);
            angleY = Mathf.Clamp(angleY, min.y, max.y);
            quaternion.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
 
            float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.z);
            angleZ = Mathf.Clamp(angleZ, min.z, max.z);
            quaternion.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
 
            return quaternion.normalized;
        }
    }
}