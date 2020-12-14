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

        public static int ClampAsIndex(this int i, ICollection target)
        {
            return Mathf.Clamp(i, 0, (target.Count - 1).Clamp0());
        }

        public static int ClampAsIndex(this int i, int length)
        {
            return Mathf.Clamp(i, 0, (length - 1).Clamp0());
        }
        
        public static int Clamp0(this int i) => Mathf.Clamp(i, 0, i);
        
        /// <param name="min">Inclusive min</param>
        /// <param name="max">Exclusive max</param>
        // ReSharper disable once InvalidXmlDocComment
        public static bool InRange(this int i, int min, int max) => i >= min && i < max;
    }
}