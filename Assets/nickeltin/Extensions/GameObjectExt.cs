using UnityEngine;

namespace nickeltin.Extensions
{
    public static class GameObjectExt
    {
        public static void Destroy(this GameObject obj) => Object.Destroy(obj);
    }
}