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

        public static Quaternion Set(this ref Quaternion quaternion, float? x = null, float? y = null, float? z = null)
        {
            quaternion.Set(x ?? quaternion.x, y ?? quaternion.y, z ?? quaternion.z, quaternion.w);
            return quaternion;
        }
    }
}