using System.Runtime.InteropServices;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class GameObjectExt
    {
        public static void Destroy(this GameObject obj, [Optional] float delay) => Object.Destroy(obj, delay);
    }
}