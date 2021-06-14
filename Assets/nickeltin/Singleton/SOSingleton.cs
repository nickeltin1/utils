using System.Linq;
using UnityEngine;

namespace nickeltin.Singletons
{
    public abstract class SOSingleton<T> : SOSBase where T : SOSingleton<T>
    {
        public static T Instance { get; private set; }

        public static bool Exists => Instance != null;

        public override bool Initialize()
        {
            if (Instance == null)
            {
                Instance = this as T;
                return true;
            }

            return false;
        }

        public override bool Destruct() => Instance == this;
    }
}