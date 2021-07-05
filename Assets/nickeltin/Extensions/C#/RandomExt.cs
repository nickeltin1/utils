using System;

namespace nickeltin.Extensions
{
    public static class RandomExt
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }

        public static bool NextBool(this Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
    }
}