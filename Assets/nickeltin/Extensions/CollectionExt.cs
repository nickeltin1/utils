using System;
using System.Collections.Generic;

namespace nickeltin.Extensions.Collections
{
    public static class CollectionExt
    {
        private static readonly System.Random random = new System.Random();
        
        public static bool InRange(this Array array, int index)
        {
            return index >= 0 && index < array.Length;
        }
        
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[random.Next(0, list.Count)];
        }
        
        public static T GetRandom<T>(this T[] list)
        {
            return list[random.Next(0, list.Length)];
        }
    }
}