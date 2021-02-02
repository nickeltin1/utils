﻿using UnityEngine;
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
        
        public static bool Negative(this Single f) => f < 0;
        public static bool Positive(this Single f) => f > 0;
        public static bool Zero(this Single f) => f == 0;
        public static bool NegativeOrZero(this Single f) => Zero(f) || Negative(f);
        public static bool PositiveOrZero(this Single f) => Zero(f) || Positive(f);
        
        public static Single Clamp(this ref Single f, Single min, Single max) => f = Mathf.Clamp(f, min, max);
        public static Single Clamp01(this ref Single f) => f = Mathf.Clamp01(f);
        
        /// <param name="min">Inclusive min</param>
        /// <param name="max">Exclusive max</param>
        // ReSharper disable once InvalidXmlDocComment
        public static bool InRange(this Single f, Single min, Single max) => f >= min && f < max;

    }
}