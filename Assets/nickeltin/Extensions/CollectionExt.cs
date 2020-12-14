using System.Collections.Generic;

namespace nickeltin.Extensions
{
    public static class CollectionExt
    {
        private static readonly System.Random random = new System.Random();
        
        public static bool InRange<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.GetLength(0);
        }

        public static bool InRange<T>(this T[,] array, int x, int y)
        {
            int xLength = array.GetLength(0);
            int yLength = array.GetLength(1);

            return x >= 0 && x < xLength && y >= 0 && y < yLength;
        }

        public static bool InRange<T>(this T[,,] array, int x, int y, int z)
        {
            int xLength = array.GetLength(0);
            int yLength = array.GetLength(1);
            int zLength = array.GetLength(2);

            return x >= 0 && x < xLength && y >= 0 && y < yLength && z >= 0 && z < zLength;
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