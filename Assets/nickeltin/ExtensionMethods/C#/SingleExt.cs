using UnityEngine;
using System;

namespace nickeltin.Extensions
{
    public static class SingleExt
    {
        public static Single To01Ratio(this Single value, Single oldRatioMin, Single oldRatioMax)
        {
            return value.ToRatio(0, 1, oldRatioMin, oldRatioMax);
        }
        
        public static Single ToRatio(this Single value, Single ratioMin, Single ratioMax, Single oldRatioMin, Single oldRatioMax)
        {
            if (oldRatioMin > oldRatioMax || ratioMin > ratioMax) return 0;
            
            Single oldRange = oldRatioMax - oldRatioMin;
            Single newRange = ratioMax - ratioMin;
            
            value = Mathf.Clamp(value, oldRatioMin, oldRatioMax);
            
            Single result = ratioMin + (newRange / (oldRange / (value - oldRatioMin)));

            return Mathf.Clamp(result, ratioMin, ratioMax);
        }

        /// <param name="x">root to be extracted form</param>
        /// <param name="n">root power, clamped to minmum of 1</param>
        public static Single Root(this Single x, int n)
        {
            n = Mathf.Clamp(n, 1, int.MaxValue);
            return Mathf.Pow(x, 1.0f / n);
        }
        
        public static bool Negative(this Single x) => x < 0;
        public static bool Positive(this Single x) => x > 0;
        public static bool Zero(this Single x) => x == 0;
        public static bool NegativeOrZero(this Single x) => Zero(x) || Negative(x);
        public static bool PositiveOrZero(this Single x) => Zero(x) || Positive(x);
        
        public static Single Clamp(this ref Single x, Single min, Single max) => x = Mathf.Clamp(x, min, max);
        public static Single ClampNoRef(this Single x, Single min, Single max) => Mathf.Clamp(x, min, max);
        
        public static Single Clamp01(this ref Single x) => x = Mathf.Clamp01(x);
        
        /// <param name="min">Inclusive min</param>
        /// <param name="max">Exclusive max</param>
        // ReSharper disable once InvalidXmlDocComment
        public static bool InRange(this Single x, Single min, Single max) => x >= min && x < max;

        public static Single Lerp(this ref Single from, Single to, Single t) => from = Mathf.Lerp(from, to, t);

        public static Single LerpNoRef(this Single from, Single to, Single t) => Mathf.Lerp(from, to, t);

        /// <summary>
        /// Finds what number <paramref name="from"></paramref> is closer to.
        /// </summary>
        /// <returns>Returns the closes point to <param name="from"></param></returns>
        public static Single ClosestPoint(this Single from, Single pointA, Single pointB)
        {
            return from.CloserToA(pointA, pointB) ? pointA : pointB;
        }
        
        /// <summary>
        /// Finds what number <paramref name="from"/> is closer to.
        /// </summary>
        /// <returns>Returns true if point to <paramref name="pointA"/> was closer, otherwise returns <paramref name="pointB"/>.</returns>
        public static bool CloserToA(this Single from, Single pointA, Single pointB)
        {
            float distA = Mathf.Abs(pointA - from);
            float distB = Mathf.Abs(pointB - from);
            return distA < distB;
        }
    }
}