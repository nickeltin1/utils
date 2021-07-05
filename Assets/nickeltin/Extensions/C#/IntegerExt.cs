using System.Collections;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class IntegerExt
    {
        public static bool Negative(this int i) => i < 0;
        public static bool Positive(this int i) => i > 0;
        public static bool Zero(this int i) => i == 0;
        public static bool NegativeOrZero(this int i) => Zero(i) || Negative(i);
        public static bool PositiveOrZero(this int i) => Zero(i) || Positive(i);

       

        public static int ClampAsIndex(this ref int i, int length) => i.Clamp0((length - 1).Clamp0NoRef());
        public static int ClampAsIndexNoRef(this int i, int length) => i.Clamp0NoRef((length - 1).Clamp0NoRef());
        public static int ClampAsIndex(this ref int i, ICollection target) => i.ClampAsIndex(target.Count);
        public static int ClampAsIndexNoRef(this int i, ICollection target) => i.ClampAsIndexNoRef(target.Count);

        
        public static int Clamp(this ref int i, int min, int max) => i = Mathf.Clamp(i, min, max);
        public static int ClampNoRef(this int i, int min, int max) => Mathf.Clamp(i, min, max);
        
        public static int Clamp0(this ref int i) => i.Clamp(0, i);
        public static int Clamp0NoRef(this int i) => i.ClampNoRef(0, i);
        public static int Clamp0(this ref int i, int max) => i.Clamp(0, max);
        public static int Clamp0NoRef(this int i, int max) => i.ClampNoRef(0, max);
        
        /// <param name="min">Inclusive min</param>
        /// <param name="max">Exclusive max</param>
        // ReSharper disable once InvalidXmlDocComment
        public static bool InRange(this int i, int min, int max) => i >= min && i < max;
    }
}