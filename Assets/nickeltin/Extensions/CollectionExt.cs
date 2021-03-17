using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace nickeltin.Extensions
{
    public static class CollectionExt
    {
        private static readonly Random random = new Random();
        
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
        
        public static T GetRandom<T>(this IList<T> list) => list[random.Next(0, list.Count)];

        public static T GetRandom<T>(this T[] list) => list[random.Next(0, list.Length)];

        public static void FillDefault<T>(this T[] array) where T : new()
        {
            for (int i = 0; i < array.GetLength(0); i++) array[i] = new T();
        }
        
        public static void FillDefault<T>(this T[,] array) where T : new()
        {
            for (int x = 0; x < array.GetLength(0); x++)
            for (int y = 0; y < array.GetLength(1); y++)
                array[x, y] = new T();
        }
        
        public static void FillDefault<T>(this T[,,] array) where T : new()
        {
            for (int x = 0; x < array.GetLength(0); x++)
            for (int y = 0; y < array.GetLength(1); y++)
            for (int z = 0; z < array.GetLength(2); z++)
                array[x,y,z] = new T();
        }

        public static void FillDefault<T>(this List<T> list, int count) where T : new()
        {
            for (int i = 0; i < count; i++) list.Add(new T());
        }

        public static IList<T> ShiftLeft<T>(this IList<T> list, int by)
        {
            for (int i = by; i < list.Count; i++) list[i - by] = list[i];
            for (int i = list.Count - by; i < list.Count; i++) list[i] = default;
            return list;
        }
        
        public static IList<T> ShiftRight<T>(this IList<T> list, int by)
        {
            for (int i = list.Count - by - 1; i >= 0; i--) list[i + by] = list[i];
            for (int i = 0; i < by; i++) list[i] = default;
            return list;
        }
        
        public static void ShiftLeft<T>(this T[] array, int by)
        {
            Array.Copy(array, by, array, 0, array.Length - by);
            Array.Clear(array, array.Length - by, by);
        }

        public static void ShiftRight<T>(this T[] array, int by)
        {
            Array.Copy(array, 0, array, by, array.Length - by);
            Array.Clear(array, 0, by);
        }

        /// <summary>
        /// Uses regural for loop
        /// </summary>
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            for (int i = 0; i < array.Length; i++) action.Invoke(array[i]);
        }
        
        /// <summary>
        /// Uses backward for loop 
        /// </summary>
        public static void ForEachReversed<T>(this T[] array, Action<T> action)
        {
            for (int i = array.Length - 1; i >= 0; i--) action.Invoke(array[i]);
        }
        
        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}